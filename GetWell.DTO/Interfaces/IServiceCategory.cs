using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GetWell.DTO.Interfaces
{
    public interface IServiceCategory : IBaseModel
    {
        [Required]
        [Display(Name = "Название", Prompt = "Название")]
        string Name { get; set; }

        [Display(Name = "Описание", Prompt = "Описание")]
        string Description { get; set; }

        int SortOrder { get; set; }

        [Display(Name = "Активный", Prompt = "Активный")]
        bool Active { get; set; }
        
        bool? IsActive { get; set; }
        
        [Display(Name = "Название Uz", Prompt = "Название Uz")]
        string NameUz { get; set; }
        
        [Display(Name = "Название En", Prompt = "Название En")]
        string NameEn { get; set; }
        
        [Display(Name = "Описание Uz", Prompt = "Описание Uz")]
        string DescriptionUz { get; set; }
        
        [Display(Name = "Описание En", Prompt = "Описание En")]
        string DescriptionEn { get; set; }

        [Display(Name = "Выберите Иконку", Prompt = "Выберите Иконку")]
        string IconUrl { get; set; }

        [Display(Name = "Выберите Иконку", Prompt = "Выберите Иконку")]
        IFormFile IconImage { get; set; }

        [Display(Name = "Цвет (нажмите чтоб выбрать)", Prompt = "Цвет (пр. #fffaaa)")]
        string HexColor { get; set; }

        [Required]
        [Display(Name = "Тип Клиники", Prompt = "Тип Клиники")]
        int CategoryID { get; set; }

        [Display(Name = "Тип Врача", Prompt = "Тип Врача")]
        int TitleID { get; set; }
    }
}