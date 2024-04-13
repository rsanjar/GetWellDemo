using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IDoctorSpecialty : IBaseModel
	{
		int DoctorID { get; set; }
		int SpecialtyID { get; set; }
		bool? IsActive { get; set; }

		[JsonIgnore]
		Doctor Doctor { get; set; }
		[JsonIgnore]
		Specialty Specialty { get; set; }
	}
}