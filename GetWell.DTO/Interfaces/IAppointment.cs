using System;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IAppointment : IBaseModel
    {
        int PatientID { get; set; }
        
        [Display(Name = "Дата Записи", Prompt = "Дата Записи")]
        DateTime AppointmentDate { get; set; }

        [Display(Name = "Время Записи", Prompt = "Время Записи")]
        TimeSpan AppointmentTime { get; set; }
        
        [Display(Name = "Отменен", Prompt = "Отменен")]
        bool IsCanceled { get; set; }
        
        [Display(Name = "Деньги Возвращены", Prompt = "Деньги Возвращены")]
        bool IsRefunded { get; set; }

        [Display(Name = "Процент Скидки", Prompt = "Процент Скидки")]
        int DiscountPercentage { get; set; }

        DateTime? AppointmentEndDate { get; set; }
        
        DateTime DateCreated { get; set; }
        
        DateTime? DateUpdated { get; set; }
        
        Guid ConfirmationCode { get; set; }
        
        DateTime? ConfirmationDate { get; set; }
        
        [Display(Name = "Подтвердить", Prompt = "Подтвердить")]
        bool? IsDoctorConfirmed { get; set; }

        [Display(Name = "Подтвердить", Prompt = "Подтвердить")]
        bool DoctorConfirmed { get; set; }

        [Display(Name = "Время Записи", Prompt = "Время Записи")]
        string AppointmentTimeStr { get; set; }
        
        [Display(Name = "Тип Услуги", Prompt = "Тип Услуги")]
        int ServiceCategoryID { get; set; }

        [Display(Name = "Услуга", Prompt = "Услуга")]
        int ServiceClinicID { get; set; }

        [Display(Name = "Врач", Prompt = "Врач")]
        int ClinicDoctorID { get; set; }

        int ServiceClinicDoctorID { get; set; }
        
        [Display(Name = "Архивирован", Prompt = "Архивирован")]
        bool IsArchived { get; set; }
        [Display(Name = "Цена (до скидки)", Prompt = "Цена (до скидки)")]
        decimal? PriceBeforeDiscount { get; set; }
        [Display(Name = "Оплачен", Prompt = "Оплачен")]
        bool IsPaid { get; set; }
    }
}