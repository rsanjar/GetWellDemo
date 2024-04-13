using System.Text.Json.Serialization;

namespace GetWell.API.UserServices
{
	public class RefreshTokenRequest
	{
		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; }
	}
}