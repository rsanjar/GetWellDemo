using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface IDoctorSpecialty : IBaseModel
{
	[Required]
	[Display(Name = "Врач", Prompt = "Врач")]
	public int DoctorID { get; set; }
	
	[Required]
	[Display(Name = "Специальность", Prompt = "Специальность")]
	public int SpecialtyID { get; set; }

	[Display(Name = "Активен", Prompt = "Активен")]
	public bool? IsActive { get; set; }
}