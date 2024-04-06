using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserRegisterModel
    {

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public DateTime? Birthdate { get; set; }

        public string? Gender { get; set; }

        public string? Citizenship { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
