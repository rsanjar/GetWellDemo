using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface IPatientFavoriteClinic : IBaseModel
{
	[Required]
	[Display(Name = "Пациент", Prompt = "Пациент")]
	public int PatientID { get; set; }
	
	[Required]
	[Display(Name = "Клиника", Prompt = "Клиника")]
	public int ClinicID { get; set; }
	
	public DateTime DateCreated { get; set; }
	
	public DateTime? DateUpdated { get; set; }
	
	public int SortOrder { get; set; }
}