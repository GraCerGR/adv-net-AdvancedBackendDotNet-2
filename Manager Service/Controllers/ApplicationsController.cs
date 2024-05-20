using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Services.Interfaces;
using Manager_Service.Models;
using Manager_Service.Models.DTO;
using User_Service.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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

        //Получить все заявки
        [HttpGet("applications")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<ApplicationPagedListModel> GetApplications([FromQuery] ApplicationSearchModel applicationSearchModel)
        {
                        string authorizationHeader = Request.Headers["Authorization"];
                        string bearerToken = authorizationHeader.Substring("Bearer ".Length);

                        var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.GetApplication(applicationSearchModel, AuthorizeuserId.ToGuid());
        }

        //Создать заявку
        [HttpPost]
        [Authorize]
        public async Task<ApplicationModel> CreateApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.CreateApplication(AuthorizeuserId.ToGuid());
        }

        //Удалить заявку
        [HttpDelete]
        [Authorize]
        public async Task DeleteApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteApplication(AuthorizeuserId.ToGuid(), null);
        }

        //Удалить заявку пользователя с id UserId
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task DeleteApplicationBy([FromBody] Guid userId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteApplication(userId, AuthorizeuserId.ToGuid());
        }

        //Назначить менеджера (себя) на заявление
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

        //Назначить менеджера (другого) на заявление
        [HttpPost("{applicationId}/assign-manager-by")]
        [Authorize(Roles = "MainManager, Admin")]
        public async Task<IActionResult> AssignManagerToApplicationBy([FromBody] Guid applicationId, [Required] Guid managerId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.ManagerApplication(applicationId, managerId);

            return Ok("Manager assigned successfully");
        }

        //Удалить менеджера (себя) с заявления
        [HttpDelete("{applicationId}/assign-manager")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> DeleteManagerToApplication([FromBody] Guid applicationId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteManagerApplication(applicationId, Guid.Parse(AuthorizeuserId));

            return Ok("You refused admission");
        }

        //Установить статус заявления
        [HttpPost("{applicationId}/status")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> SetStatus([FromBody] Guid applicationId, Status status)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.SetStatus(applicationId, Guid.Parse(AuthorizeuserId), status);

            return Ok("Status assigned successfully");
        }
    }
}
