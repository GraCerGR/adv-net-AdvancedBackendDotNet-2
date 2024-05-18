﻿using Microsoft.AspNetCore.Authorization;
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

        public ProgramsController(IProgramsService programsService, IUserService userService)
        {
            _programsService = programsService;
            _userService = userService;
        }


        [HttpGet("program")]
        //[Authorize]
        public async Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms([FromQuery] ProgramSearchModel programSearchModel)
        {
            return await _programsService.GetPrograms(programSearchModel);
        }

        [HttpPost("queue")]
        [Authorize]
        public async Task CreateQueuePrograms(Guid? userId, List<Guid> programs)
        {
            
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            // Проверка прав, что id в Authorize является Менеджером userId
            // Если userId не введён, то userId = id из Authorize

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _programsService.CreateQueuePrograms(userId ?? AuthorizeuserId.ToGuid(), programs);
        }
    }
    public static class GuidExtensions
    {
        public static Guid ToGuid(this object obj)
        {
            if (obj != null)
            {
                return Guid.Parse(obj.ToString());
            }
            throw new ArgumentNullException(nameof(obj), "Object is null");
        }
    }
}
