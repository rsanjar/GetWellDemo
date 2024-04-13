using System;

namespace GetWell.Data.Model.Interface
{
	public interface ISpecialty : IDateCreatedLoggable, IBaseModel
	{
		string Name { get; set; }
		int SortOrder { get; set; }
		bool? IsActive { get; set; }
		string Description { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
	}
}