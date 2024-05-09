using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Handbook_Service.Models
{
    public class EducationDocumentTypeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        public EducationLevelModel EducationLevel { get; set; }
        //public List<EducationLevelModel> NextEducationLevels { get; set; }
    }
}
