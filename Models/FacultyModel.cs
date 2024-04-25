using System.ComponentModel.DataAnnotations;

namespace Handbook_Service.Models
{
    public class FacultyModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
