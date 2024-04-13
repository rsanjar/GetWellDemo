using System;
using GetWell.DTO;

namespace GetWell.API.Models;

public class AppointmentRequest
{
	public int ServiceClinicDoctorID { get; set; }
	
	public int AppointmentYear { get; set; }

	public int AppointmentMonth { get; set; }

	public int AppointmentDay { get; set; }

	public int AppointmentHour { get; set; }

	public int AppointmentMinute { get; set; }
	
	public int? ClinicDiscountID { get; set; }

    public static explicit operator Appointment(AppointmentRequest model)
    {
        return new Appointment
        {
            ServiceClinicDoctorID = model.ServiceClinicDoctorID,
            AppointmentDate = new DateTime(model.AppointmentYear, model.AppointmentMonth, model.AppointmentDay),
            AppointmentTime = new TimeSpan(model.AppointmentHour, model.AppointmentMinute, 0),
            DiscountPercentage = 0,
            ConfirmationCode = Guid.NewGuid(),
            IsArchived = false,
            DoctorConfirmed = false,
            IsPaid = false
        };
    }
}