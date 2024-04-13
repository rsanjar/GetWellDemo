using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GetWell.DTO.Interfaces;

public interface ICity : IBaseModel
{
	[Required]
	[Display(Name = "Название Города", Prompt = "Название Города")]
	public string Name { get; set; }
	
	public int SortOrder { get; set; }

	[Required]
	[Display(Name = "Страна", Prompt = "Страна")]
	public int CountryID { get; set; }
	
	[Display(Name = "Название Города Узб.", Prompt = "Название Города Узб.")]
	public string NameUz { get; set; }
	
	[Display(Name = "Название Города Англ.", Prompt = "Название Города Англ.")]
	public string NameEn { get; set; }
}