using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Services.Interfaces;
using Manager_Service.Models;
using Manager_Service.Models.DTO;
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

        [HttpGet("applications")]
        //[Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<ApplicationPagedListModel> GetApplications([FromQuery] ApplicationSearchModel applicationSearchModel)
        {
                        string authorizationHeader = Request.Headers["Authorization"];
                        string bearerToken = authorizationHeader.Substring("Bearer ".Length);

                        var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.GetApplication(applicationSearchModel, AuthorizeuserId.ToGuid());
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

        [HttpDelete]
        [Authorize]
        public async Task DeleteApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteApplication(AuthorizeuserId.ToGuid());
        }

        [HttpPost("{applicationId}/assign-manager")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> AssignManagerToApplication([FromBody] Guid applicationId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.ManagerApplication(applicationId, Guid.Parse(AuthorizeuserId));

            return Ok("Manager assigned successfully");
        }

/*        [HttpPost("{applicationId}/assign-manager-by")]
        [Authorize(Roles = "MainManager, Admin")]
        public async Task<IActionResult> AssignManagerToApplicationBy([FromBody] Guid applicationId, [FromBody] Guid managerId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.ManagerApplication(applicationId, managerId);

            return Ok("Manager assigned successfully");
        }*/
    }
}
