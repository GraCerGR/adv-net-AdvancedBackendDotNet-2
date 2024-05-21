using System.ComponentModel.DataAnnotations;
using User_Service.Models.DTO;

namespace User_Service.Models
{
    public class AdminModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

    }
}
