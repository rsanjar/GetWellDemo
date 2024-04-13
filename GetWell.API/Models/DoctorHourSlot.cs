using Newtonsoft.Json;
using System.Collections.Generic;

namespace GetWell.API.Models;

public class DoctorHourSlot
{
	public DoctorHourSlot(int hour)
	{
		Hour = hour;
		TimeSlots = new List<DoctorMinuteSlot>();
	}

	[JsonProperty]
	public int Hour { get; }
	
	[JsonProperty]
	public List<DoctorMinuteSlot> TimeSlots { get; set; }
}