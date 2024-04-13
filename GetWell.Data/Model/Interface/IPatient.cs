using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IPatient : IDateLoggable, IBaseModel
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string MiddleName { get; set; }
		string Phone { get; set; }
		string SecondaryPhone { get; set; }
		string Email { get; set; }
		bool IsActive { get; set; }
		int PreferredLanguage { get; set; }
        int? CityID { get; set; }
		
		[JsonIgnore]
		PatientAccount PatientAccount { get; set; }
		PatientProfile PatientProfile { get; set; }
		[JsonIgnore]
		ICollection<Appointment> Appointments { get; set; }
		[JsonIgnore]
		ICollection<ClinicReview> ClinicReviews { get; set; }
	}
}