using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces;

public interface ITitle : IBaseModel
{
	[Required]
	[Display(Name = "Профессия", Prompt = "Профессия")]
	public string Name { get; set; }
	
	public int SortOrder { get; set; }
	
	public bool? IsActive { get; set; }
	
	[Display(Name = "Профессия Узб.", Prompt = "Профессия Узб.")]
	public string NameUz { get; set; }
	
	[Display(Name = "Профессия Англ.", Prompt = "Профессия Англ.")]
	public string NameEn { get; set; }
}