using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface ICountry : IBaseModel
{
	[Required]
	[Display(Name = "Страна", Prompt = "Страна")]
	public string Name { get; set; }
	
	[Display(Name = "Страна Узб.", Prompt = "Страна Узб.")]
	public string NameUz { get; set; }
	
	[Display(Name = "Страна Англ.", Prompt = "Страна Англ.")]
	public string NameEn { get; set; }
}