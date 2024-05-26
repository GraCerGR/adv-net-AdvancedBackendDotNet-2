using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using User_Service.Services.Interfaces;
using User_Service.Models;
using User_Service.Models.DTO;
using User_Service.Services;

namespace Manager_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IUserService _userService;

        public ManagerController(IManagerService managerService, IUserService userService)
        {
            _managerService = managerService;
            _userService = userService;
        }


        [HttpPost("manager")]
        [Authorize(Roles = "Admin")]
        public async Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel)
        {
            return await _managerService.CreateManagers(managerCreateModel);
        }

        [HttpGet("manager")]
        [Authorize(Roles = "MainManager, Admin")]
        public async Task<List<ManagerDto>> GetManagers()
        {
            // Получаем значение заголовка "Authorization"
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var userId = await _userService.GetUserIdFromToken(bearerToken);
            
            await _userService.GetProfile(userId); // - Существует такой пользователь или нет

            return await _managerService.GetManagers();
        }
    }
}