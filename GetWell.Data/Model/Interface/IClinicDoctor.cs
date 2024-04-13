using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicDoctor : IDateCreatedLoggable, IBaseModel
	{
		int ClinicID { get; set; }
		int DoctorID { get; set; }
		bool? IsActive { get; set; }
		DateTime? DateDisabled { get; set; }
		Clinic Clinic { get; set; }
		Doctor Doctor { get; set; }

		ClinicDoctorAccount ClinicDoctorAccount { get; set; }
		[JsonIgnore]
		ICollection<ClinicDoctorWorkDay> ClinicDoctorWorkDays { get; set; }
	}
}