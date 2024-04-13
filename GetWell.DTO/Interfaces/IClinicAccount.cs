using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces
{
    public interface IClinicAccount : IAccount
    {
        [Required]
        [DataType(DataType.Password)]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [Compare(nameof(Password))]
        [Display(Name = "Подтвердить Пароль", Prompt = "Подтвердить Пароль")]
        public string ConfirmPassword { get; set; }

        int ClinicID { get; set; }
    }
}