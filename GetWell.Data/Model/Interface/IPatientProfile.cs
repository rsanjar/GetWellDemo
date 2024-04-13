using System;

namespace GetWell.Data.Model.Interface
{
	public interface IPatientProfile : IDateLoggable, IBaseModel
	{
		int PatientID { get; set; }
		DateTime? DateOfBirth { get; set; }
		string Address { get; set; }
		string Street { get; set; }
		string District { get; set; }
		int? RegionID { get; set; }
		bool IsMale { get; set; }
		Region Region { get; set; }
        string PhotoBase64 { get; set; }
	}
}