using System.ComponentModel.DataAnnotations;

namespace Handbook_Service.Models
{
    public class EducationLevelModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


    }
}
