using System.Text.Json.Serialization;

namespace Document_Service.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FileTypes
    {
        Passport,
        EducationFile
    }
}
