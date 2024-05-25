using System.Linq;
using Manager_Service.Context;
using Manager_Service.Models;
using Manager_Service.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using User_Service.Models.DTO;
using Manager_Service.Models.DTO;
using User_Service.Models;
using User_Service.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Manager_Service.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly ApplicationContext _context;
        private readonly User_Service.Context.ApplicationContext _contextU;
        private readonly Handbook_Service.Context.ApplicationContext _contextH;

        public ApplicationsService(ApplicationContext context, User_Service.Context.ApplicationContext contextU, Handbook_Service.Context.ApplicationContext contextH)
        {
            _context = context;
            _contextU = contextU;
            _contextH = contextH;
        }

        public async Task<ApplicationModel> CreateApplication(Guid userId)
        {
            var queue = await _context.QueuePrograms.FirstOrDefaultAsync(q => q.UserId.ToString() == userId.ToString());
            if (queue == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), $"The program queue was not found");
                throw ex;
            }

            var user = await _contextU.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());
            if (user == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "User not exists");
                throw ex;
            }

/*            if (FindUser(userId) == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "User not exists");
                throw ex;
            }*/

            var ExistingApplicant = await _context.Applications.FirstOrDefaultAsync(a => a.Applicant == userId);
            if (ExistingApplicant != null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "The application already exists. To create a new one, delete the previous one");
                throw ex;
            }

/*            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                Citizenship = user.Citizenship,
                PhoneNumber = user.PhoneNumber,
            };*/

            ApplicationModel application = new ApplicationModel
            {
                Id = Guid.NewGuid(),
                Applicant = userId,
                QueueProgram = queue,
                Manager = null,
                Status = Status.created,
                LastModification = DateTime.UtcNow,
            };

            try
            {
                await _context.Applications.AddAsync(application);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine("Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            return application;
        }

        public async Task DeleteApplication(Guid userId, Guid? managerId)
        {
            var user = await _contextU.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());
            if (user == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "User not exists");
                throw ex;
            }
            //Если заявка закрыта
            if (user.ApplicationStatus == true)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status403Forbidden.ToString(), "The application is closed. The change is not possible");
                throw ex;
            }

            var ExistingApplicant = await _context.Applications.FirstOrDefaultAsync(a => a.Applicant == userId);
            if (ExistingApplicant == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "The application was not found");
                throw ex;
            }

            if (managerId != null)
            {
                if (ExistingApplicant.Manager != managerId)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "You are not the manager of this application");
                    throw ex;
                }
            }

            _context.Applications.RemoveRange(_context.Applications.Where(ap => ap.Applicant == userId));
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationPagedListModel> GetApplication(ApplicationSearchModel applicationSearchModel, Guid userId)
        {
            //Делать 1 запрос в рамках разных контекестов нельзя.

            // Получение всех заявок
            var applications = await _context.Applications.Include(app => app.QueueProgram).ToListAsync();

            // Получение всех аппликантов
            var applicants = await _contextU.Users.ToListAsync();

            // Фильтрация по имени аппликанта
            if (!string.IsNullOrEmpty(applicationSearchModel.Name))
            {
                applications = applications.Where(app =>
                {
                    var applicant = applicants.FirstOrDefault(u => u.Id == app.Applicant);
                    return applicant != null && applicant.Name.Contains(applicationSearchModel.Name);
                }).ToList();
            }

            if (!string.IsNullOrEmpty(applicationSearchModel.ProgramId))
            {
                var programIds = _context.QueuePrograms.Where(q => q.Queue.Contains(Guid.Parse(applicationSearchModel.ProgramId))).Select(q => q.Id).ToList();
                applications = applications.Where(app => programIds.Contains(app.QueueProgram.Id)).ToList();
            }

            if (applicationSearchModel.faculty != null && applicationSearchModel.faculty.Any())
            {
                var facultyNames = applicationSearchModel.faculty;
                applications = applications.Where(app =>
                {
                    return app.QueueProgram.Queue.Any(q =>
                        _contextH.EducationPrograms.Any(p => p.Id == q && facultyNames.Contains(p.Faculty.Name))
                    );
                }).ToList();
            }

            if (applicationSearchModel.AdmissionStatus != null)
            {
                applications = applications.Where(app => app.Status == applicationSearchModel.AdmissionStatus).ToList();
            }

            if (applicationSearchModel.hasNotManager)
            {
                applications = applications.Where(app => app.Manager == null).ToList();
            }

            if (applicationSearchModel.myApplicants)
            {
                applications = applications.Where(app => app.Manager == userId).ToList();
            }

            if (applicationSearchModel.sortByDate == Sort.Asc)
            {
                applications = applications.OrderBy(app => app.LastModification).ToList();
            }
            else if (applicationSearchModel.sortByDate == Sort.Desc)
            {
                applications = applications.OrderByDescending(app => app.LastModification).ToList();
            }

            int page = applicationSearchModel.Page;
            int size = applicationSearchModel.Size;
            var pageCount = (int)Math.Ceiling((double)applications.Count / size);
            var paginatedApplications = applications.Skip((page - 1) * size).Take(size).ToList();


            var applicationDtos = paginatedApplications.Select(app => new ApplicationDto
            {
                Id = app.Id,
                Applicant = UserModelToDto(applicants.FirstOrDefault(u => u.Id == app.Applicant)),
                QueueProgram = app.QueueProgram,
                Manager = app.Manager != null ? UserModelToDto(applicants.FirstOrDefault(u => u.Id == app.Manager)) : null,
                Status = app.Status,
                LastModification = app.LastModification,
            }).ToList();

            var applicationPageListModel = new ApplicationPagedListModel
            {
                Applications = applicationDtos,
                Pagination = new PageInfoModel
                {
                    Size = size,
                    Count = pageCount,
                    Current = page
                }
            };

            return applicationPageListModel;
        }

        public async Task ManagerApplication(Guid appplicationId, Guid managerId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(q => q.Id.ToString() == appplicationId.ToString());
            if (application == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), $"The appplication was not found");
                throw ex;
            }

            var manager = await _contextU.Managers.FirstOrDefaultAsync(u => u.UserId.ToString() == managerId.ToString());
            if (manager == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "Manager not exists");
                throw ex;
            }

            if (application.Manager != null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "The application already has a manager");
                throw ex;
            }

            application.Manager = managerId;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine("Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            return;
        }

        public async Task DeleteManagerApplication(Guid appplicationId, Guid managerId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(q => q.Id.ToString() == appplicationId.ToString());
            if (application == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), $"The appplication was not found");
                throw ex;
            }

            var manager = await _contextU.Managers.FirstOrDefaultAsync(u => u.UserId.ToString() == managerId.ToString());
            if (manager == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "Manager not exists");
                throw ex;
            }

            if (application.Manager == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "The application does not have a manager");
                throw ex;
            }

            if (application.Manager != managerId)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "You are not the manager of this application");
                throw ex;
            }

            application.Manager = null;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine("Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            return;
        }

        public async Task<MessageDto> SetStatus(Guid appplicationId, Guid managerId, Status status)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(q => q.Id.ToString() == appplicationId.ToString());
            if (application == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), $"The appplication was not found");
                throw ex;
            }

            var manager = await _contextU.Managers.FirstOrDefaultAsync(u => u.UserId.ToString() == managerId.ToString());
            if (manager == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "Manager not exists");
                throw ex;
            }

            if (application.Manager != managerId)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "You are not the manager of this application");
                throw ex;
            }

            if (application.Status == status)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "The application has already been assigned this status");
                throw ex;
            }

            application.Status = status;

            var user = await _contextU.Users.FirstOrDefaultAsync(u => u.Id.ToString() == application.Applicant.ToString());

            if (status == Status.closed)
            {
                user.ApplicationStatus = true;
            }
            else
            {
                user.ApplicationStatus = false;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine("Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            var messageData = new MessageDto
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Message = $"The status of your application has changed to: {application.Status}"
            };

            return messageData;
        }

        private UserDto UserModelToDto(UserModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Email = userModel.Email,
                Birthdate = userModel.Birthdate,
                Gender = userModel.Gender,
                Citizenship = userModel.Citizenship,
                PhoneNumber = userModel.PhoneNumber
            };
        }

/*        public async Task FindUser(Guid userId)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };


            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //Console.WriteLine("Письмо отправлено");

            channel.QueueDeclare(queue: "UserCheckQueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userId));

            channel.BasicPublish(exchange: "",
            routingKey: "UserCheckQueue",
            basicProperties: null,
            body: body);
        }*/
    }
}
