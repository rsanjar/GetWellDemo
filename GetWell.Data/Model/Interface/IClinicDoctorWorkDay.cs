using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicDoctorWorkDay : IBaseModel
	{
		int WeekDayID { get; set; }
		int ClinicDoctorID { get; set; }
		TimeSpan StartTime { get; set; }
		TimeSpan EndTime { get; set; }
		TimeSpan? BreakStartTime { get; set; }
		TimeSpan? BreakEndTime { get; set; }
	}
}