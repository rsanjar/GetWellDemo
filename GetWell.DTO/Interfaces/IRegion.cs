using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface IRegion : IBaseModel
{
	[Required]
	[Display(Name = "Регион", Prompt = "Регион")]
	public string Name { get; set; }
	
	[Required]
	[Display(Name = "Город", Prompt = "Город")]
	public int CityID { get; set; }
	
	public int SortOrder { get; set; }
	
	[Display(Name = "Город Узб.", Prompt = "Город Узб.")]
	public string NameUz { get; set; }
	
	[Display(Name = "Город Англ.", Prompt = "Город Англ.")]
	public string NameEn { get; set; }
}