using System;

namespace GetWell.Data.Model.Interface;

public interface IClinicDoctorEvent : IDateLoggable, IBaseModel
{
    int ClinicDoctorID { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    DateTime AppointmentStartDate { get; set; }
    TimeSpan AppointmentStartTime { get; set; }
    DateTime? AppointmentEndDate { get; set; }
    bool IsCompleted { get; set; }
    bool IsCanceled { get; set; }
    ClinicDoctor ClinicDoctor { get; set; }
}