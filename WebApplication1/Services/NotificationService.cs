using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using User_Service.Models.DTO;
using User_Service.Context;
using User_Service.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace User_Service.Services
{
    public class NotificationService : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public NotificationService(IServiceProvider sp)
        {
            _sp = sp;

            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "EmailQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var emailData = JsonConvert.DeserializeObject<MessageDto>(message);
                SendEmail(emailData);

/*                Task.Run(() =>
                {
                    var emailData = JsonConvert.DeserializeObject<MessageDto>(message);
                    SendEmail(emailData);
                });*/
            };

            _channel.BasicConsume(queue: "EmailQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public async Task SendEmail(MessageDto messageData)
        {
            MailAddress from = new MailAddress("lk.gs.applicant@gmail.com", "Личный кабинет ТГУ");
            MailAddress toAddress = new MailAddress($"{messageData.Email}");

            MailMessage mailMessage = new MailMessage(from, toAddress);
            mailMessage.Subject = "Verification";
            mailMessage.Body = $"{messageData.Message}";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(from.Address, "iadw bgpe hrfm twcq");
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
