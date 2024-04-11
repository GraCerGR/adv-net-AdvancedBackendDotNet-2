using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Models.DTO;
using WebApplication1.Services.Interfaces;
using RabbitMQ.Client;

namespace WebApplication1.Services
{
    public class NotificationService: INotificationService
    {
        private readonly ApplicationContext _context;

        public NotificationService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task SendNotificationRabbitMQ(MessageDto messageData)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "EmailQueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            var message = JsonConvert.SerializeObject(messageData);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
            routingKey: "EmailQueue",
            basicProperties: null,
            body: body);
        }

    }
}
