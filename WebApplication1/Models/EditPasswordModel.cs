using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
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
