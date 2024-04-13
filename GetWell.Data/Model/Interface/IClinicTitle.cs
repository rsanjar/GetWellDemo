using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicTitle : IDateCreatedLoggable, IBaseModel
	{
		int ClinicID { get; set; }
		int TitleID { get; set; }
		int SortOrder { get; set; }
		bool? IsActive { get; set; }
	}
}