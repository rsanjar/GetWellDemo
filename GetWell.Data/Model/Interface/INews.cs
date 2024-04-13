using System;

namespace GetWell.Data.Model.Interface
{
	public interface INews : IDateLoggable, IBaseModel
	{
		string Title { get; set; }
		string Body { get; set; }
		bool IsDisabled { get; set; }
		bool IsFeatured { get; set; }
		string BannerUrl { get; set; }
		int? NewsLanguage { get; set; }
	}
}