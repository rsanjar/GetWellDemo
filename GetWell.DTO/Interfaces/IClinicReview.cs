using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface IClinicReview : IBaseModel
{
	[Required]
	[Display(Name = "Пациент", Prompt = "Пациент")]
	public int PatientID { get; set; }
	
	[Required]
	[Display(Name = "Рейтинг", Prompt = "Рейтинг")]
	public int Rating { get; set; }
	
	[Required]
	[Display(Name = "Заголовок", Prompt = "Заголовок")]
	public string ReviewTitle { get; set; }
	
	[Required]
	[Display(Name = "Отзыв", Prompt = "Отзыв")]
	public string Review { get; set; }
	
	[Required]
	[Display(Name = "Клиника", Prompt = "Клиника")]
	public int ClinicID { get; set; }
	
	[Display(Name = "Отключен", Prompt = "Отключен")]
	public bool IsDisabled { get; set; }
	
	[Display(Name = "Заблокирован", Prompt = "Заблокирован")]
	public bool IsBlocked { get; set; }
	
	public DateTime DateCreated { get; set; }
	
	public DateTime? DateUpdated { get; set; }
	
	[Display(Name = "Язык", Prompt = "Язык")]
	public int? ReviewLanguage { get; set; }
}