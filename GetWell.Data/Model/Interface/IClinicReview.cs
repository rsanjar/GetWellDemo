using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicReview : IDateLoggable, IBaseModel
	{
		int PatientID { get; set; }
		int Rating { get; set; }
		string ReviewTitle { get; set; }
		string Review { get; set; }
		int ClinicID { get; set; }
		bool IsDisabled { get; set; }
		bool IsBlocked { get; set; }
		int? ReviewLanguage { get; set; }
	}
}