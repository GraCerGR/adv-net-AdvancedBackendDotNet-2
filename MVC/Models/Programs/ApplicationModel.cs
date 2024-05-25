using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.Models.Programs
{
    public class ApplicationModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Applicant { get; set; }

        public QueueProgramsModel QueueProgram { get; set; }

        public Guid? Manager { get; set; }

        public Status Status { get; set; }

        public DateTime LastModification { get; set; }
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
