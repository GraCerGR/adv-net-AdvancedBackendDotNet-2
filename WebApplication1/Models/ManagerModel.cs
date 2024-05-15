using System.ComponentModel.DataAnnotations;
using User_Service.Models.DTO;

namespace User_Service.Models
{
    public class ManagerModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool MainManager { get; set; } = false;
    }
}
