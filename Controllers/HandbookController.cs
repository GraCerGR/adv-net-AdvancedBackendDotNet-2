using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Handbook_Service.Models;
using Handbook_Service.Services.Interfaces;
using Handbook_Service.Models;
using System.Text.Json;

namespace Handbook_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandbookController : ControllerBase
    {
        private readonly IHandbookService _handbookService;

        public HandbookController(IHandbookService handbookService)
        {
            _handbookService = handbookService;
        }

        [HttpGet("EducationLevels")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportEducationLevels()
        {
            var educationLevels = await _handbookService.GetEducationLevels();
            return Ok(educationLevels);
        }

        [HttpGet("Faculties")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportFaculties()
        {
            var faculties = await _handbookService.GetFaculties();
            return Ok(faculties);
        }
    }
}
