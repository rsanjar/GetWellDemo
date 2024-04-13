using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GetWell.DTO.Interfaces
{
    public interface ICategory : IBaseModel
    {
		[Required]
		[Display(Name = "Название", Prompt = "Название")]
	    public string Name { get; set; }
	    
	    [Display(Name = "Описание", Prompt = "Описание")]
	    public string Description { get; set; }
	    
	    [Display(Name = "Иконка", Prompt = "Иконка")]
	    public string IconUrl { get; set; }
	    
	    [Display(Name = "Активен", Prompt = "Активен")]
	    public bool? IsActive { get; set; }

	    [Display(Name = "Иконка", Prompt = "Иконка")]
	    public IFormFile IconImage { get; set; }

	    [Display(Name = "Активен", Prompt = "Активен")]
		public bool Active { get; set; }
	    
	    [Display(Name = "Цвет", Prompt = "Цвет")]
	    public string HexColor { get; set; }
	    
	    [Display(Name = "Сортировочный номер", Prompt = "Сортировочный номер")]
	    public int SortOrder { get; set; }
	    
	    [Display(Name = "Название Uz", Prompt = "Название Uz")]
	    public string NameUz { get; set; }
	    
	    [Display(Name = "Название En", Prompt = "Название En")]
	    public string NameEn { get; set; }
	    
	    [Display(Name = "Описание Uz", Prompt = "Описание Uz")]
	    public string DescriptionUz { get; set; }
	    
	    [Display(Name = "Описание En", Prompt = "Описание En")]
	    public string DescriptionEn { get; set; }
	    
	    public DateTime DateCreated { get; set; }
	    
	    public DateTime? DateUpdated { get; set; }
    }
}