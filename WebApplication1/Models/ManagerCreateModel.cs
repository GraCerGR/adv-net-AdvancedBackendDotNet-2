using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ManagerCreateModel
    {
        [Required]
        public Guid Id { get; set; }

        //[Required]
        public bool MainManager { get; set; } = false;
    }
}
