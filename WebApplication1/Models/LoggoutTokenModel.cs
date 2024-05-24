using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class LoggoutTokenModel
    {
        [Key]
        public Guid Id { get; set; }
        public string AccessToken { get; set; }

    }
}
