using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface INews : IBaseModel
    {
        [Required]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Описание", Prompt = "Описание")]
        public string Body { get; set; }
        
        [Display(Name = "Спрятать", Prompt = "Спрятать")]
        public bool IsDisabled { get; set; }
        
        [Display(Name = "Избранное", Prompt = "Избранное")]
        public bool IsFeatured { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public DateTime? DateUpdated { get; set; }
        
        [Display(Name = "Баннер", Prompt = "Баннер")]
        public string BannerUrl { get; set; }
        
        [Display(Name = "Язык", Prompt = "Язык")]
        public int? NewsLanguage { get; set; }
    }
}