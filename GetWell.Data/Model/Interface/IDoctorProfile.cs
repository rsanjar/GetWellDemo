using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IDoctorProfile : IBaseModel
	{
		int DoctorID { get; set; }
		int PreferredLanguageID { get; set; }
		string About { get; set; }
		string ProfilePictureUrl { get; set; }
		string HomeAddress { get; set; }
		string Street { get; set; }
		string District { get; set; }
		int RegionID { get; set; }
		bool IsMale { get; set; }
		string MedicalTitle { get; set; }
		string TaxpayerID { get; set; }
		string AboutUz { get; set; }
		string AboutEn { get; set; }
		string MedicalTitleUz { get; set; }
		string MedicalTitleEn { get; set; }

		[JsonIgnore]
		Doctor Doctor { get; set; }
		[JsonIgnore]
		Region Region { get; set; }
	}
}