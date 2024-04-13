using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GetWell.DTO.Interfaces
{
    public interface IClinicGallery : IBaseModel
    {
        int ClinicID { get; set; }

        [Required]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        string Title { get; set; }
        
        [Display(Name = "Выберите Фото", Prompt = "Выберите Фото")]
        string ImageUrl { get; set; }
        
        bool IsHidden { get; set; }

        int SortOrder { get; set; }

        [Display(Name = "Главное Фото", Prompt = "Главное Фото")]
        bool IsThumbnail { get; set; }

        [Display(Name = "Для Мобильных", Prompt = "Для Мобильных")]
        bool IsMobileImage { get; set; }
        
        DateTime DateCreated { get; set; }
        
        DateTime? DateUpdated { get; set; }
        
        string TitleUz { get; set; }
        
        string TitleEn { get; set; }

        [Display(Name = "Выберите Фото", Prompt = "Выберите Фото")]
        IFormFile Image { get; set; }

        string ImageFileName { get; set; }
    }
}