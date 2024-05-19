using Manager_Service.Context;
using Manager_Service.Models;
using Manager_Service.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using User_Service.Models.DTO;

namespace Manager_Service.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly ApplicationContext _context;
        private readonly User_Service.Context.ApplicationContext _contextU;

        public ApplicationsService(ApplicationContext context, User_Service.Context.ApplicationContext contextU)
        {
            _context = context;
            _contextU = contextU;
        }

        public async Task<ApplicationModel> CreateApplication(Guid userId)
        {
            var queue = await _context.QueuePrograms.FirstOrDefaultAsync(u => u.UserId.ToString() == userId.ToString());
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
                ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "User not exists");
                throw ex;
            }

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                Citizenship = user.Citizenship,
                PhoneNumber = user.PhoneNumber,
            };

            var ExistingApplicant = await _context.Applications.FirstOrDefaultAsync(u => u.Applicant == userDto);
            if (user != null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "The application already exists. To create a new one, delete the previous one");
                throw ex;
            }

            ApplicationModel application = new ApplicationModel
            {
                Id = Guid.NewGuid(),
                Applicant = userDto,
                QueueProgram = queue,
                Manager = null,
                Status = "created"
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
    }
}
