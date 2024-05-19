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
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationsService _applicationsService;
        private readonly IUserService _userService;

        public ApplicationsController(IApplicationsService applicationsService, IUserService userService)
        {
            _applicationsService = applicationsService;
            _userService = userService;
        }


        [HttpPost]
        [Authorize]
        public async Task<ApplicationModel> CreateApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.CreateApplication(AuthorizeuserId.ToGuid());
        }

/*        [HttpDelete]
        [Authorize]
        public async Task<ApplicationModel> DeleteApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.DeleteApplication(AuthorizeuserId.ToGuid());
        }*/
    }
}
