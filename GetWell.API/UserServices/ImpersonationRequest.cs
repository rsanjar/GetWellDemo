using System.Text.Json.Serialization;

namespace GetWell.API.UserServices
{
	public class ImpersonationRequest
	{
		[JsonPropertyName("username")]
		public string UserName { get; set; }
	}
}