using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IPatient : IBaseModel
    {
        [Required]
        [Display(Name = "Имя", Prompt = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        public string LastName { get; set; }
        
        [Display(Name = "Отчество", Prompt = "Отчество")]
        public string MiddleName { get; set; }
        
        [Required]
        [Display(Name = "Телефон", Prompt = "Сотовый Телефон")]
        public string Phone { get; set; }
        
        [Display(Name = "Дополнительный Телефон", Prompt = "Дополнительный Телефон")]
        public string SecondaryPhone { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Эл. Почта", Prompt = "Эл. Почта")]
        public string Email { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public DateTime? DateUpdated { get; set; }
        
        [Display(Name = "Активировано", Prompt = "Активировано")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Язык", Prompt = "Язык")]
        public int PreferredLanguage { get; set; }

        [Display(Name = "Город", Prompt = "Город")]
        public int? CityID { get; set; }
    }
}