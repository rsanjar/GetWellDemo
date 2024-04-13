using Newtonsoft.Json;

namespace GetWell.API.Models;

public class DoctorMinuteSlot
{
	public DoctorMinuteSlot(bool isOccupied, int minuteBlock)
	{
		IsOccupied = isOccupied;
		MinuteBlock = minuteBlock;
	}
	
	[JsonProperty]
	public int MinuteBlock { get; }

	[JsonProperty]
	public bool IsOccupied { get; }
}