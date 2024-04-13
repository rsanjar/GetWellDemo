using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GetWell.DTO.Interfaces
{
    public interface IDoctorProfile : IBaseModel
    {
        int DoctorID { get; set; }

        [Display(Name = "Предпочитаемый Язык", Prompt = "Предпочитаемый Язык")]
        int PreferredLanguageID { get; set; }

        [Display(Name = "О Себе", Prompt = "О Себе")]
        string About { get; set; }

        [Display(Name = "Выберите Фото", Prompt = "Выберите Фото")]
        string ProfilePictureUrl { get; set; }

        [Display(Name = "Выберите Фото", Prompt = "Выберите Фото")]
        IFormFile ProfilePicture { get; set; }

        [Display(Name = "Адрес", Prompt = "Адрес")]
        string HomeAddress { get; set; }

        [Display(Name = "Улица", Prompt = "Улица")]
        string Street { get; set; }

        [Display(Name = "Массив", Prompt = "Массив")]
        string District { get; set; }

        [Required]
        [Display(Name = "Город", Prompt = "Город")]
        int CityID { get; set; }

        [Required]
        [Display(Name = "Район", Prompt = "Район")]
        int RegionID { get; set; }

        [Display(Name = "Пол", Prompt = "Пол")]
        bool IsMale { get; set; }

        [Display(Name = "Название Должности", Prompt = "Название Должности")]
        string MedicalTitle { get; set; }

        [Display(Name = "ИНН", Prompt = "ИНН")]
        string TaxpayerID { get; set; }
        string AboutUz { get; set; }
        string AboutEn { get; set; }
        string MedicalTitleUz { get; set; }
        string MedicalTitleEn { get; set; }
    }
}