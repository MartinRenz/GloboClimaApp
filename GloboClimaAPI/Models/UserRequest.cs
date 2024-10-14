using System.Text.Json.Serialization;

namespace GloboClimaAPI.Models
{
    public class UserRequest
    {
        [JsonPropertyName("login")]
        public string? Login { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
