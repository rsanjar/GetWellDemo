using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IRegion : IBaseModel
	{
		string Name { get; set; }
		int CityID { get; set; }
		int SortOrder { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }

		City City { get; set; }
		[JsonIgnore]
		ICollection<Clinic> Clinics { get; set; }
		[JsonIgnore]
		ICollection<DoctorProfile> DoctorProfiles { get; set; }
		[JsonIgnore]
		ICollection<PatientProfile> PatientProfiles { get; set; }
		[JsonIgnore]
		ICollection<ZipCode> ZipCodes { get; set; }
	}
}