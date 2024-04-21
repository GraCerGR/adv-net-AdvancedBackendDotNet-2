using System.ComponentModel.DataAnnotations;

namespace Document_Service.Models.DTO
{
    public class PassportDto
    {

        [Required]
        public string SeriesNumber { get; set; }

        [Required]
        public string Birthplace { get; set; }

        [Required]
        public DateTime WhenIssued { get; set; }

        [Required]
        public string IssuedByWhom { get; set; }
    }
}
