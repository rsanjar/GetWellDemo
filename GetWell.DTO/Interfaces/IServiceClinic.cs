using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IServiceClinic : IBaseModel
    {
        int ClinicID { get; set; }

        int ServiceID { get; set; }

        string Name { get; set; }

        [Display(Name = "Цена Услуги", Prompt = "В сумах")]
        decimal? Price { get; set; }

        [Display(Name = "Цена Услуги (в сумах)", Prompt = "В сумах")]
        int? PriceInt { get; set; }

        [Display(Name = "Цена Услуги (в сумах)", Prompt = "В сумах")]
        string PriceString { get; set; }

        [Display(Name = "Включить", Prompt = "Включить")]
        bool Active { get; set; }
        
        bool? IsActive { get; set; }
        
        TimeSpan AverageDuration { get; set; }

        [Display(Name = "Продолжительность", Prompt = "В минутах")]
        int? Duration { get; set; }
        
        int SortOrder { get; set; }
        
        DateTime DateCreated { get; set; }
        
        DateTime? DateUpdated { get; set; }
    }
}