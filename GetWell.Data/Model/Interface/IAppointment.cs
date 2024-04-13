using System;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IAppointment : IDateLoggable, IBaseModel
	{
		int PatientID { get; set; }
		int ServiceClinicDoctorID { get; set; }
		DateTime AppointmentDate { get; set; }
		TimeSpan AppointmentTime { get; set; }
		bool IsCanceled { get; set; }
		bool IsRefunded { get; set; }
		int DiscountPercentage { get; set; }
		DateTime? AppointmentEndDate { get; set; }
		Guid ConfirmationCode { get; set; }
		DateTime? ConfirmationDate { get; set; }
		bool? IsDoctorConfirmed { get; set; }
		bool IsArchived { get; set; }
		decimal? PriceBeforeDiscount { get; set; }
		bool IsPaid { get; set; }

		[JsonIgnore]
		ServiceClinicDoctor ServiceClinicDoctor { get; set; }
		
		[JsonIgnore]
		Patient Patient { get; set; }
		
		AppointmentProfile AppointmentProfile { get; set; }
	}
}