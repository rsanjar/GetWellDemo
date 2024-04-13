using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO.Interfaces
{
    public interface IClinicPhone : IBaseModel
    {
        [Display(Name = "Телефон")]
        string Phone { get; set; }
        
        [Display(Name = "Основной")]
        bool IsMain { get; set; }

        [Display(Name = "Мобильный")]
        bool? IsMobile { get; set; }

        [Display(Name = "Отключен")]
        bool IsDisabled { get; set; }
        
        int SortOrder { get; set; }

        int ClinicID { get; set; }
    }
}