using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface IPatientFavoriteDoctor : IBaseModel
{
	[Required]
	[Display(Name = "Пациент", Prompt = "Пациент")]
	public int PatientID { get; set; }

	[Required]
	[Display(Name = "Доктор", Prompt = "Доктор")]
	public int ClinicDoctorID { get; set; }
	
	public DateTime DateCreated { get; set; }
	
	public DateTime? DateUpdated { get; set; }

	public int SortOrder { get; set; }
}