using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class EditUserModel
    {

        [MinLength(1)]
        public string? Name { get; set; }

        [MinLength(1)]
        [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
        public string? Email { get; set; }

        public DateTime? Birthdate { get; set; }

        public string? Gender { get; set; }

        public string? Citizenship { get; set; }

        [Phone(ErrorMessage = "Некорректный формат номера телефона.")]
        public string? PhoneNumber { get; set; }

    }
}
