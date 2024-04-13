using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GetWell.DTO.Interfaces
{
	public interface IAccount : IBaseModel
	{
		[Required]
		[DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер Телефона", Prompt = "Номер Телефона")]
		string MobilePhone { get; set; }
        
		[Required]
		[JsonIgnore]
		[DataType(DataType.Password)]
		[System.Text.Json.Serialization.JsonIgnore]
        [Display(Name = "Пароль", Prompt = "Пароль")]
		string Password { get; set; }
        
        [Display(Name = "Активирован", Prompt = "Активирован")]
        bool IsActive { get; set; }
		
        bool IsPhoneVerified { get; set; }
		
        DateTime DateCreated { get; set; }
		
        DateTime? DateUpdated { get; set; }
		
        DateTime? LastLoginDate { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        Guid UniqueKey { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        int? SmsActivationCode { get; set; }
		
        DateTime? SmsActivationDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Эл. Почта", Prompt = "Эл. Почта")]
        string Email { get; set; }
		
        bool IsEmailVerified { get; set; }
		
        DateTime? EmailVerificationDate { get; set; }
        public DateTime? LastVerificationAttemptDate { get; set; }
	}
}