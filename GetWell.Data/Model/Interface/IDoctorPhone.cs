using System;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IDoctorPhone : IDateLoggable, IBaseModel
	{
		string Phone { get; set; }
		bool? IsMobile { get; set; }
		bool IsPrimary { get; set; }
		bool IsWorkPhone { get; set; }
		bool? IsActive { get; set; }
		int DoctorID { get; set; }

		[JsonIgnore]
		Doctor Doctor { get; set; }
	}
}