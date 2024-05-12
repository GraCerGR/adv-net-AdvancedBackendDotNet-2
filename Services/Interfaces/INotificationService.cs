using NotificationService.Models;
using NotificationService.Models.DTO;

namespace NotificationService.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationRabbitMQ(MessageDto message);
    }
}
