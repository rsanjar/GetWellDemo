using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.API.Models;

public class PhoneLoginRequest
{
    [Required]
    public string Phone { get; set; }

    [Required]
    [Range(10000, 99999)]
    public int SmsActivationCode { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public int AccountID { get; set; }
}