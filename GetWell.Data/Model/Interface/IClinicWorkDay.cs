using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicWorkDay : IBaseModel
	{
		int WeekDayID { get; set; }
		int ClinicID { get; set; }
		TimeSpan OpenTime { get; set; }
		TimeSpan CloseTime { get; set; }
	}
}