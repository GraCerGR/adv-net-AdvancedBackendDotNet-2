using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Services.Interfaces;
using Manager_Service.Models;

namespace Manager_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramsService _programsService;

        public ProgramsController(IProgramsService programsService)
        {
            _programsService = programsService;
        }


        [HttpGet("program")]
        //[Authorize]
        public async Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms([FromQuery] ProgramSearchModel programSearchModel)
        {
            return await _programsService.GetPrograms(programSearchModel);
        }
    }
}
