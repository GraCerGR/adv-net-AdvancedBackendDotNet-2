using User_Service.Models;
using User_Service.Models.DTO;

namespace User_Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationRabbitMQ(MessageDto message);
    }
}
