using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IServiceClinicDoctor : IBaseModel
    {
        int ServiceClinicID { get; set; }
        
        int ClinicDoctorID { get; set; }
        
        bool? IsActive { get; set; }

        [Display(Name = "Включить", Prompt = "Включить")]
        bool Active { get; set; }

        TimeSpan? AverageDuration { get; set; }

        [Display(Name = "Продолжительность", Prompt = "В минутах")]
        int? Duration { get; set; }

        [Display(Name = "Цена Услуги", Prompt = "В сумах")]
        decimal? Price { get; set; }

        [Display(Name = "Цена Услуги", Prompt = "В сумах")]
        int? PriceInt { get; set; }
        
        string Description { get; set; }
        
        string DescriptionUz { get; set; }
        
        string DescriptionEn { get; set; }
        
        DateTime DateCreated { get; set; }
        
        DateTime? DateUpdated { get; set; }
    }
}