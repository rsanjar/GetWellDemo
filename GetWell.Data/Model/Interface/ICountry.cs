using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface ICountry : IBaseModel
	{
		string Name { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }

		[JsonIgnore]
		ICollection<City> Cities { get; set; }
	}
}