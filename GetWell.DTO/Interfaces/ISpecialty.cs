using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface ISpecialty : IBaseModel
{
	[Required]
	[Display(Name = "Специальность", Prompt = "Специальность")]
	public string Name { get; set; }

	public int SortOrder { get; set; }

	[Display(Name = "Включен", Prompt = "Включен")]
	public bool? IsActive { get; set; }
	
	public DateTime DateCreated { get; set; }
	
	[Display(Name = "Описание", Prompt = "Описание")]
	public string Description { get; set; }
	
	[Display(Name = "Специальность Узб.", Prompt = "Специальность Узб.")]
	public string NameUz { get; set; }
	
	[Display(Name = "Специальность Англ.", Prompt = "Специальность Англ.")]
	public string NameEn { get; set; }
}