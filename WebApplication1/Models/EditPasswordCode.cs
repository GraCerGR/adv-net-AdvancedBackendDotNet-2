using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class EditPasswordCode
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
