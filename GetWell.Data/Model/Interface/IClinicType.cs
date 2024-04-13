using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicType : IDateLoggable, IBaseModel
	{
		string Name { get; set; }
		string Description { get; set; }
		string IconUrl { get; set; }
		bool? IsActive { get; set; }
		string HexColor { get; set; }
		int SortOrder { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
		string DescriptionUz { get; set; }
		string DescriptionEn { get; set; }
	}
}