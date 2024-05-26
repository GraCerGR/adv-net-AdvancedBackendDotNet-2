using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Services.Interfaces;
using Manager_Service.Models;
using Manager_Service.Models.DTO;
using User_Service.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using User_Service.Models.DTO;

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

            return await _applicationsService.GetApplication(applicationSearchModel, Guid.Parse(AuthorizeuserId));
        }

        //Создать заявку
        [HttpPost]
        [Authorize]
        public async Task<ApplicationModel> CreateApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            return await _applicationsService.CreateApplication(Guid.Parse(AuthorizeuserId));
        }

        //Удалить заявку
        [HttpDelete]
        [Authorize]
        public async Task<string> DeleteApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteApplication(Guid.Parse(AuthorizeuserId), null);

            return "The application was deleted successfully";
        }

        //Удалить заявку пользователя с id UserId
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<string> DeleteApplicationBy([FromRoute] Guid userId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            await _applicationsService.DeleteApplication(userId, Guid.Parse(AuthorizeuserId));

            return "The application was deleted successfully";
        }

        //Назначить менеджера (себя) на заявление
        [HttpPost("{applicationId}/assign-manager")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> AssignManagerToApplication([FromRoute] Guid applicationId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            List<MessageDto> messageData = await _applicationsService.ManagerApplication(applicationId, Guid.Parse(AuthorizeuserId));

            await _userService.SendNotificationRabbitMQ(messageData[0]);

            await _userService.SendNotificationRabbitMQ(messageData[1]);

            return Ok("Manager assigned successfully");
        }

        //Назначить менеджера (другого) на заявление
        [HttpPost("{applicationId}/assign-manager-by")]
        [Authorize(Roles = "MainManager, Admin")]
        public async Task<IActionResult> AssignManagerToApplicationBy([FromRoute] Guid applicationId, [Required] Guid managerId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            List<MessageDto> messageData = await _applicationsService.ManagerApplication(applicationId, managerId);

            await _userService.SendNotificationRabbitMQ(messageData[0]);

            await _userService.SendNotificationRabbitMQ(messageData[1]);

            return Ok("Manager assigned successfully");
        }

        //Удалить менеджера (себя) с заявления
        [HttpDelete("{applicationId}/assign-manager")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> DeleteManagerToApplication([FromRoute] Guid applicationId)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            List<MessageDto> messageData = await _applicationsService.DeleteManagerApplication(applicationId, Guid.Parse(AuthorizeuserId));

            await _userService.SendNotificationRabbitMQ(messageData[0]);

            await _userService.SendNotificationRabbitMQ(messageData[1]);

            return Ok("You refused admission");
        }

        //Установить статус заявления
        [HttpPost("{applicationId}/status")]
        [Authorize(Roles = "Manager, MainManager, Admin")]
        public async Task<IActionResult> SetStatus([FromRoute] Guid applicationId, Status status)
        {
            string authorizationHeader = Request.Headers["Authorization"];
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var AuthorizeuserId = await _userService.GetUserIdFromToken(bearerToken);

            MessageDto messageData =  await _applicationsService.SetStatus(applicationId, Guid.Parse(AuthorizeuserId), status);

            await _userService.SendNotificationRabbitMQ(messageData);

            return Ok("Status assigned successfully");
        }
    }
}
