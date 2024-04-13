using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GetWell.DTO.Interfaces
{
    public interface IService : IBaseModel
    {
        [Required]
        [Display(Name = "Название", Prompt = "Название")]
        string Name { get; set; }

        [Display(Name = "Описание", Prompt = "Опишите услугу тут")]
        string Description { get; set; }

        [Required]
        [Display(Name = "Тип Услуги", Prompt = "Тип Услуги")]
        int ServiceCategoryID { get; set; }

        [Display(Name = "Активный", Prompt = "Активный")]
        bool Active { get; set; }

        bool? IsActive { get; set; }
        
        int SortOrder { get; set; }
        
        [Display(Name = "Название Uz", Prompt = "Название Uz")]
        string NameUz { get; set; }
        
        [Display(Name = "Название En", Prompt = "Название En")]
        string NameEn { get; set; }
        
        [Display(Name = "Описание Uz", Prompt = "Опишите услугу тут Uz")]
        string DescriptionUz { get; set; }
        
        [Display(Name = "Описание En", Prompt = "Опишите услугу тут En")]
        string DescriptionEn { get; set; }

        [Display(Name = "Выберите Иконку", Prompt = "Выберите Иконку")]
        string IconUrl { get; set; }

        [Display(Name = "Выберите Иконку", Prompt = "Выберите Иконку")]
        IFormFile IconImage { get; set; }

    }
}