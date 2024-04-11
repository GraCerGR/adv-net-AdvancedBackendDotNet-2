using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationRabbitMQ(MessageDto message);
    }
}
