using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces
{
    public interface IClinicWorkDay : IBaseModel
    {
        [Required]
        int WeekDayID { get; set; }
        int ClinicID { get; set; }
        TimeSpan OpenTime { get; set; }
        TimeSpan CloseTime { get; set; }

        [Display(Name = "Начало")]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        TimeSpan? OpenTimeNullable { get; set; }

        [Display(Name = "Конец")]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        TimeSpan? CloseTimeNullable { get; set; }
    }
}