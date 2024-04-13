using System;

namespace GetWell.DTO.Interfaces;

public interface IClinicDoctor : IBaseModel
{
    int ServiceClinicDoctorID { get; set; }
	public int ClinicID { get; set; }
	public int DoctorID { get; set; }
	public bool? IsActive { get; set; }
    public DateTime DateCreated { get; set; }
	public DateTime? DateDisabled { get; set; }
}