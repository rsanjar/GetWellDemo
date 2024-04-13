using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IServiceClinic : IDateLoggable, IBaseModel
	{
		int ClinicID { get; set; }
		int ServiceID { get; set; }
		decimal? Price { get; set; }
		bool? IsActive { get; set; }
		TimeSpan AverageDuration { get; set; }
		int SortOrder { get; set; }
		
		[JsonIgnore]
		Service Service { get; set; }
    }
}