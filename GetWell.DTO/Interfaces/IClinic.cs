using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO.Interfaces
{
    public interface IClinic : IBaseModel
    {
        [Display(Name = "Выберите Логотип", Prompt = "Выберите Логотип")]
        public IFormFile LogoImage { get; set; }

        [Display(Name = "Выберите Фото", Prompt = "Выберите Фото")]
        public IFormFile ThumbnailImage { get; set; }

        [Required]
        [Display(Name = "Название Клиники", Prompt = "Название Клиники")]
        string Name { get; set; }

        [Display(Name = "Вебсайт", Prompt = "Вебсайт")]
        string Website { get; set; }
        
        [Required]
        [Display(Name = "Улица", Prompt = "Улица")]
        string Street { get; set; }

        [Required]
        [Display(Name = "Адрес", Prompt = "Адрес")]
        string Address { get; set; }

        [Display(Name = "Ориентир", Prompt = "Ориентир")]
        string NearBy { get; set; }

        [Display(Name = "Широта", Prompt = "Широта")]
        double Latitude { get; set; }

        [Display(Name = "Долгота", Prompt = "Долгота")]
        double Longitude { get; set; }
        
        [Display(Name = "Широта", Prompt = "Широта")]
        public string Lat { get; set; }
        
        [Display(Name = "Долгота", Prompt = "Долгота")]
        public string Long { get; set; }
        
        [Required]
        [Display(Name = "Район", Prompt = "Район")]
        int RegionID { get; set; }

        string RegionName { get; set; }

        string FullAddress { get; }
        
        [Required]
        [Display(Name = "Город", Prompt = "Город")]
        int CityID { get; set; }

        [Display(Name = "Этаж", Prompt = "Этаж")]
        int? Floor { get; set; }

        [Display(Name = "Ближайшее Метро", Prompt = "Ближайшее Метро")]
        string NearestSubway { get; set; }

        [Display(Name = "Частная", Prompt = "Частная Клиника")]
        bool Private { get; set; }
        
        bool? IsPrivate { get; set; }
        
        [Display(Name = "Главная Клиника", Prompt = "Главная Клиника")]
        int? ParentDepartmentID { get; set; }
        
        [Display(Name = "Название Клиники Uz", Prompt = "Название Клиники Uz")]
        string NameUz { get; set; }
        
        [Display(Name = "Название Клиники En", Prompt = "Название Клиники En")]
        string NameEn { get; set; }
        
        [Display(Name = "Массив", Prompt = "Массив")]
        string District { get; set; }

        [Display(Name = "Массив Uz", Prompt = "Массив Uz")]
        string DistrictUz { get; set; }

        [Display(Name = "Массив En", Prompt = "Массив En")]
        string DistrictEn { get; set; }

        [Display(Name = "Улица Uz", Prompt = "Улица Uz")]
        string StreetUz { get; set; }
        
        [Display(Name = "Улица En", Prompt = "Улица En")]
        string StreetEn { get; set; }

        [Display(Name = "Адрес Uz", Prompt = "Адрес Uz")]
        string AddressUz { get; set; }
       
        [Display(Name = "Адрес En", Prompt = "Адрес En")]
        string AddressEn { get; set; }

        [Display(Name = "Ориентир Uz", Prompt = "Ориентир Uz")]
        string NearByUz { get; set; }

        [Display(Name = "Ориентир En", Prompt = "Ориентир En")]
        string NearByEn { get; set; }
       
        [Display(Name = "Ближайшее Метро Uz", Prompt = "Ближайшее Метро Uz")]
        string NearestSubwayUz { get; set; }
        
        [Display(Name = "Ближайшее Метро En", Prompt = "Ближайшее Метро En")]
        string NearestSubwayEn { get; set; }
        
        [Display(Name = "Описание Клиники", Prompt = "Опишите клинику для пользователей")]
        string Description { get; set; }
        
        [Display(Name = "Описание Клиники Uz", Prompt = "Опишите клинику для пользователей Uz")]
        string DescriptionUz { get; set; }
        
        [Display(Name = "Описание Клиники En", Prompt = "Опишите клинику для пользователей En")]
        string DescriptionEn { get; set; }
        
        DateTime DateCreated { get; set; }
        
        DateTime? DateUpdated { get; set; }

        [Display(Name = "Избранная Клиника", Prompt = "Избранная Клиника")]
        bool IsFeatured { get; set; }

        [Display(Name = "Логотип", Prompt = "Логотип")]
        string LogoUrl { get; set; }
        
        [Display(Name = "Фото", Prompt = "Фото")]
        string ThumbnailUrl { get; set; }
        
        double Rating { get; set; }

        int ReviewCount { get; set; }
        
        string CityName { get; set; }
        
        string CityNameEn { get; set; }
        
        string CityNameUz { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        Guid UniqueKey { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        string QrImageCode { get; set; }

        int DiscountPercentage { get; set; }

        DateTime? BusinessStartDate { get; set; }
        DateTime? BusinessEndDate { get; set; }
        bool? IsActive { get; set; }

        [Display(Name = "Сортировка", Prompt = "Сортировка в приложении")]
        int SortOrder { get; set; }

        [JsonIgnore]
        ClinicAccount Account { get; set; }

        [JsonIgnore]
        List<ClinicPhone> Phones { get; set; }

        [JsonIgnore]
        List<ClinicWorkDay> WorkDay { get; set; }

        [JsonIgnore]
        List<SelectListItem> Cities { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        List<ClinicGallery> Gallery { get; set; }
    }
}