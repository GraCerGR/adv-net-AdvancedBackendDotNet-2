using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.DTO;

namespace WebApplication1.Models
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
