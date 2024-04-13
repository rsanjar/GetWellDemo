using System;

namespace GetWell.DTO.Interfaces;

public interface IClinicDoctorEvent : IBaseModel
{
    public int ClinicDoctorID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime AppointmentStartDate { get; set; }
    public TimeSpan AppointmentStartTime { get; set; }
    public DateTime? AppointmentEndDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCanceled { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}