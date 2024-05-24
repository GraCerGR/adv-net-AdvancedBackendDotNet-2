using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models.Handbook
{
    public class FacultyModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
