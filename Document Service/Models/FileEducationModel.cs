using System.ComponentModel.DataAnnotations;

namespace Document_Service.Models
{
    public class FileEducationModel
    {
        [Key]
        public Guid Id { get; set; }

        //public string FileName { get; set; }

        public string PathToFile { get; set; }

        public Guid UserId { get; set; }

        public string DocumentTypes { get; set; }
    }
}
