using System;

namespace GetWell.Data.Model.Interface
{
	public interface IDoctorTitle : IDateCreatedLoggable, IBaseModel
	{
		int DoctorID { get; set; }
		int TitleID { get; set; }
		bool? IsPrimaryTitle { get; set; }
		bool? IsActive { get; set; }
	}
}