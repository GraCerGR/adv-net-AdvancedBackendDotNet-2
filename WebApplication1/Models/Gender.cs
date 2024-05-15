using System.Text.Json.Serialization;

namespace User_Service.Models
{
    
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Gender
        {
            Male,
            Female
        }
    
}
