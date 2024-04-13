using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces
{
    public interface IClinicDoctorWorkDay : IBaseModel
    {
        int WeekDayID { get; set; }
        int ClinicDoctorID { get; set; }

        [Display(Name = "Начало")]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        TimeSpan? OpenTimeNullable { get; set; }

        [Display(Name = "Конец")]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        TimeSpan? CloseTimeNullable { get; set; }
        TimeSpan StartTime { get; set; }
        TimeSpan EndTime { get; set; }
        
        [Display(Name = "Перерыв От")]
        TimeSpan? BreakStartTime { get; set; }
        
        [Display(Name = "Перерыв До")]
        TimeSpan? BreakEndTime { get; set; }
    }
}