using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
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
        public async Task<ManagerCreateModel[]> CreateManagers(ManagerCreateModel[] managerCreateModel)
        {
            return await _managerService.CreateManagers(managerCreateModel);
        }
    }
}
