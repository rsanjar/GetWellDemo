using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces
{
    public interface IPatientProfile : IBaseModel
    {
        public int PatientID { get; set; }

        [Display(Name = "Дата Рождения", Prompt = "Дата Рождения")]
        public DateTime? DateOfBirth { get; set; }
        
        [Display(Name = "Адрес", Prompt = "Адрес")]
        public string Address { get; set; }
        
        [Display(Name = "Улица", Prompt = "Улица")]
        public string Street { get; set; }
        
        [Display(Name = "Массив", Prompt = "Массив")]
        public string District { get; set; }
        
        [Display(Name = "Город", Prompt = "Город")]
        public int CityID { get; set; }

        [Display(Name = "Район", Prompt = "Район")]
        public int? RegionID { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public DateTime? DateUpdated { get; set; }
        
        [Display(Name = "Мужчина", Prompt = "Мужчина")]
        public bool IsMale { get; set; }

        [Display(Name = "Фото", Prompt = "Фото")]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        string PhotoBase64 { get; set; }
    }
}