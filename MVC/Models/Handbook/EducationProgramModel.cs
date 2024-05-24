using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models.Handbook
{
    public class EducationProgramModel
    {
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]

        public Guid Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string Code { get; set; }

        [Required]
        [MinLength(1)]
        public string Language { get; set; }

        [Required]
        [MinLength(1)]
        public string EducationForm { get; set; }

        public FacultyModel Faculty { get; set; }

        public EducationLevelModel EducationLevel { get; set; }
    }
}
