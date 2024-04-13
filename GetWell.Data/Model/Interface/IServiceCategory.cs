using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IServiceCategory : IBaseModel
	{
		string Name { get; set; }
		string Description { get; set; }
		int SortOrder { get; set; }
		bool? IsActive { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
		string DescriptionUz { get; set; }
		string DescriptionEn { get; set; }
		string IconUrl { get; set; }
		string HexColor { get; set; }
		int CategoryID { get; set; }
		int TitleID { get; set; }

		Category Category { get; set; }
		Title Title { get; set; }
		ICollection<Service> Services { get; set; }
	}
}