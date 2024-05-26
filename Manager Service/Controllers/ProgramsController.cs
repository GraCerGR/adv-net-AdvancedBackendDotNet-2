using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Services.Interfaces;
using Manager_Service.Models;
using User_Service.Services.Interfaces;

namespace Manager_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramsService _programsService;
        private readonly IUserService _userService;
        private readonly IApplicationsService _applicationsService;

        public ProgramsController(IProgramsService programsService, IUserService userService, IApplicationsService applicationsService)
        {
            _programsService = programsService;
            _userService = userService;
            _applicationsService = applicationsService;
        }

        //Создать приоритет программ (свой)
        [HttpPost("queue")]
        [Authorize]
        public async Task<IActionResult> CreateQueuePrograms(List<Guid> programs)
        {
            
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            // Если userId не введён, то userId = id из Authorize

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _programsService.CreateQueuePrograms(Guid.Parse(AuthorizeuserId), programs, null);
        }

        //Создать чужой приоритет программ (пользователю с id userId)
        [HttpPost("queue/{userId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task CreateQueuePrograms(Guid userId, List<Guid> programs)
        {

            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            // если userId введён, то проверка прав, что id в Authorize является Менеджером пользователя userId

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _programsService.CreateQueuePrograms(userId, programs, Guid.Parse(AuthorizeuserId));
        }

        //Получить свой приоритет программ
        [HttpGet("queue")]
        [Authorize]
        public async Task<IQueryable<QueueProgramsModel>> GetQueuePrograms()
        {

            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            // если userId введён, то проверка прав, что id в Authorize является просто Менеджером
            // Если userId не введён, то userId = id из Authorize

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _programsService.GetQueuePrograms(Guid.Parse(AuthorizeuserId));
        }

        //Получить чужой приоритет программ (пользователя с id userId)
        [HttpGet("queue/{userId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IQueryable<QueueProgramsModel>> GetQueuePrograms(Guid userId)
        {

            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            // если userId введён, то проверка прав, что id в Authorize является просто Менеджером
            // Если userId не введён, то userId = id из Authorize

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _programsService.GetQueuePrograms(userId);
        }

        //Получить все программы (конфликт с Get в Application)
        [HttpGet("programs")]
        //[Authorize]
        public async Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms([FromQuery] ProgramSearchModel programSearchModel)
        {
            return await _programsService.GetPrograms(programSearchModel);
        }
    }
}
