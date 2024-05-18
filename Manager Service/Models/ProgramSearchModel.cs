using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager_Service.Models
{
    public class ProgramSearchModel
    {
        public string? Faculty { get; set; }
        public string? EducationLevel { get; set; }
        public string? EducationForm { get; set; }
        public string? Language { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int Size { get; set; } = 5;
    }
}
