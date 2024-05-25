using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.Models.Programs
{
    public class ApplicationSearchModel
    {
        public string? Name { get; set; }
        public string? ProgramId { get; set; }
        public List<string>? faculty { get; set; }
        public Status? AdmissionStatus { get; set; }
        public bool hasNotManager { get; set; } = false;
        public bool myApplicants { get; set; } = false;
        public Sort sortByDate { get; set; } = Sort.Asc;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int Size { get; set; } = 5;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Sort
    {
        Asc,
        Desc
    }
}
