using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IDoctor : IDateLoggable, IBaseModel
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string MiddleName { get; set; }
		string FirstNameUz { get; set; }
		string LastNameUz { get; set; }
		string MiddleNameUz { get; set; }
		string FirstNameEn { get; set; }
		string LastNameEn { get; set; }
		string MiddleNameEn { get; set; }
		string Email { get; set; }
		DateTime DateOfBirth { get; set; }
		DateTime? RetirementDate { get; set; }
		bool? IsActive { get; set; }
		bool IsRetired { get; set; }
		bool IsFamilyDoctor { get; set; }
		DateTime? CareerStartDate { get; set; }
		
		DoctorProfile DoctorProfile { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicDoctor> ClinicDoctors { get; set; }
		
		[JsonIgnore]
		ICollection<DoctorPhone> DoctorPhones { get; set; }
		
		[JsonIgnore]
		ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }
	}
}