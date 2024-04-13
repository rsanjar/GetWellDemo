using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GetWell.Core;

namespace GetWell.API.Models
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            Role = UserRoles.Patient; //default role
        }

        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonIgnore]
        public int AccountID { get; set; }
    }
}