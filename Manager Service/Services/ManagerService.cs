/*using Manager_Service.Context;
using Manager_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Models.DTO;
using Manager_Service.Models;

namespace Manager_Service.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ApplicationContext _context;
        private readonly User_Service.Context.ApplicationContext _contextU;

        public ManagerService(ApplicationContext context, User_Service.Context.ApplicationContext contextU)
        {
            _context = context;
            _contextU = contextU;
        }

        public async Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel)
        {
            foreach (var managerModel in managerCreateModel)
            {
                var userEntity = await _contextU.Users.FirstOrDefaultAsync(x => x.Id == managerModel.Id);
                if (userEntity == null)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "User not found");
                    throw ex;
                }

                var managerEntity = await _contextU.Managers.FirstOrDefaultAsync(x => x.UserId == managerModel.Id);
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

                try
                {
                    await _contextU.Managers.AddAsync(manager);
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
    }
}
*/