using System.ComponentModel.DataAnnotations;
using User_Service.Models.DTO;

namespace Manager_Service.Models.DTO
{
    public class ApplicationDto
    {
        public Guid Id { get; set; }

        public UserDto Applicant { get; set; }

        public QueueProgramsModel QueueProgram { get; set; }

        public UserDto? Manager { get; set; }

        public Status Status { get; set; }

        public DateTime LastModification { get; set; }
    }
}
