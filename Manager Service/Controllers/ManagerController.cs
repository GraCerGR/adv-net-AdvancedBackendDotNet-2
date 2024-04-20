using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController: ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }


        [HttpPost("manager")]
        [Authorize(Roles = "Admin")]
        public async Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel)
        {
            return await _managerService.CreateManagers(managerCreateModel);
        }
    }
}
