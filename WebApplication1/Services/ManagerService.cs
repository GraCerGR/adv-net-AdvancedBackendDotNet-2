using User_Service.Context;
using User_Service.Models;
using User_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using User_Service.Models.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace User_Service.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ApplicationContext _context;

        public ManagerService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel)
        {
            foreach (var managerModel in managerCreateModel)
            {
                var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == managerModel.Id);
                if (userEntity == null)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "User not found");
                    throw ex;
                }

                var managerEntity = await _context.Managers.FirstOrDefaultAsync(x => x.UserId == managerModel.Id);
                if (managerEntity != null)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status409Conflict.ToString(), "User " + managerModel.Id + " is already a manager. Previous users are assigned");
                    throw ex;
                }

                ManagerModel manager = new ManagerModel
                {
                    Id = Guid.NewGuid(),
                    UserId = managerModel.Id,
                    MainManager = managerModel.MainManager,
                };

                var messageData = new MessageDto
                {
                    Id = Guid.NewGuid(),
                    Email = userEntity.Email,
                    Message = $"You have been appointed as a manager"
                };

                try
                {
                    await _context.Managers.AddAsync(manager);
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
                    // Дополнительная обработка ошибки, логирование и т.д.
                }
            }

            return managerCreateModel;
        }

        public async Task<List<ManagerDto>> GetManagers()
        {
            var managers = await _context.Managers.ToListAsync();

            var managerDtos = new List<ManagerDto>();

            foreach (var manager in managers)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == manager.UserId);

                if (user != null)
                {
                    var userDto = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Birthdate = user.Birthdate,
                        Gender = user.Gender,
                        Citizenship = user.Citizenship,
                        PhoneNumber = user.PhoneNumber
                    };

                    var managerDto = new ManagerDto
                    {
                        Id = manager.Id,
                        UserId = userDto,
                        MainManager = manager.MainManager
                    };

                    managerDtos.Add(managerDto);
                }
            }

            return managerDtos;
        }
    }
}
