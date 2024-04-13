using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    interface IDoctor : IBaseModel
    {
        [Display(Name = "Выбрать Клинику", Prompt = "Выбрать Клинику")]
        int? ClinicID { get; set; }

        [Required]
        [Display(Name = "Имя", Prompt = "Имя")]
        string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        string LastName { get; set; }

        [Required]
        [Display(Name = "Отчество", Prompt = "Отчество")]
        string MiddleName { get; set; }

        [Required]
        [Display(Name = "Эл. Почта", Prompt = "Эл. Почта")]
        string Email { get; set; }

        [Display(Name = "Дата Рождения", Prompt = "Дата Рождения")]
        DateTime DateOfBirth { get; set; }

        [Display(Name = "Дата Ухода в Пенсию", Prompt = "Дата Ухода в Пенсию")]
        DateTime? RetirementDate { get; set; }

        [Display(Name = "Активен", Prompt = "Активен")]
        bool? IsActive { get; set; }

        [Display(Name = "Активен", Prompt = "Активен")]
        bool Active { get; set; }

        [Display(Name = "В Пенсии", Prompt = "В Пенсии")]
        bool IsRetired { get; set; }

        [Display(Name = "Семейный Врач", Prompt = "Семейный Врач")]
        bool IsFamilyDoctor { get; set; }

        [Display(Name = "Начало Карьеры", Prompt = "Начало Карьеры")]
        DateTime? CareerStartDate { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DateUpdated { get; set; }
        string FirstNameUz { get; set; }
        string LastNameUz { get; set; }
        string MiddleNameUz { get; set; }
        string FirstNameEn { get; set; }
        string LastNameEn { get; set; }
        string MiddleNameEn { get; set; }
    }
}