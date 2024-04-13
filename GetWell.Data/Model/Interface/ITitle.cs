using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface ITitle : IBaseModel
	{
		string Name { get; set; }
		int SortOrder { get; set; }
		bool? IsActive { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }

		[JsonIgnore]
		ICollection<ServiceCategory> ServiceCategories { get; set; }
	}
}