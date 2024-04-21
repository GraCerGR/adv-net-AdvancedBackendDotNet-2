using System.ComponentModel.DataAnnotations;

namespace Document_Service.Models
{
    public class FilePassportModel
    {
        [Key]
        public Guid Id { get; set; }

        //public string FileName { get; set; }

        public string PathToFile { get; set; }

        public Guid UserId { get; set; }

        public string SeriesNumber { get; set; }

        public string Birthplace { get; set; }

        public DateTime WhenIssued { get; set; }

        public string IssuedByWhom { get; set; }
    }
}
