using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using User_Service.Models.DTO;

namespace Manager_Service.Models
{
    public class ApplicationModel
    {
        [Key]
        public Guid Id { get; set; }

        public UserDto Applicant { get; set; }

        public QueueProgramsModel QueueProgram { get; set; }

        public UserDto? Manager { get; set; }

        public string Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        created, 
        consideration, 
        confirmed, 
        rejected, 
        closed
    }
}
