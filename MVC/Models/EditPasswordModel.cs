using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class EditPasswordModel
    {
        [Required]
        [MinLength(6)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
