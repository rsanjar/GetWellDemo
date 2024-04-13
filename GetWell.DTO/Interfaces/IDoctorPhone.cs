using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IDoctorPhone : IBaseModel
    {
        [Display(Name = "Телефон")]
        string Phone { get; set; }
        
        [Display(Name = "Основной")]
        bool IsPrimary { get; set; }

        [Display(Name = "Мобильный")]
        bool? IsMobile { get; set; }

        [Display(Name = "Рабочий Телефон")] 
        bool IsWorkPhone { get; set; }

        [Display(Name = "Активен")]
        bool? IsActive { get; set; }

        int DoctorID { get; set; }
    }
}