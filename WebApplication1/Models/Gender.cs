using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Gender
        {
            Male,
            Female
        }
    
}
