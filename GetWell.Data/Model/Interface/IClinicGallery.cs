using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicGallery : IDateLoggable, IBaseModel
	{
		int ClinicID { get; set; }
		string Title { get; set; }
		string ImageUrl { get; set; }
		bool IsHidden { get; set; }
		int SortOrder { get; set; }
		bool IsThumbnail { get; set; }
		bool IsMobileImage { get; set; }
		string TitleUz { get; set; }
		string TitleEn { get; set; }
	}
}