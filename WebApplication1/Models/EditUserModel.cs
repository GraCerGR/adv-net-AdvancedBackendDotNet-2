using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class EditUserModel
    {

        [MinLength(1)]
        public string? Name { get; set; }

        [MinLength(1)]
        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? Birthdate { get; set; }

        public string? Gender { get; set; }

        public string? Citizenship { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

    }
}
