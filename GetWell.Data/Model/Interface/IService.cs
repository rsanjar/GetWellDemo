using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GetWell.Data.Model.Interface
{
	public interface IService : IBaseModel
	{
		string Name { get; set; }
		string Description { get; set; }
		int ServiceCategoryID { get; set; }
		bool? IsActive { get; set; }
		int SortOrder { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
		string DescriptionUz { get; set; }
		string DescriptionEn { get; set; }
        string IconUrl { get; set; }

		ServiceCategory ServiceCategory { get; set; }
		
		[JsonIgnore]
		ICollection<ServiceClinic> ServiceClinics { get; set; }
	}
}