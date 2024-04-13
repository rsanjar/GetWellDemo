namespace GetWell.API.Models;

public class SmsConfig
{
	public string BaseUrl { get; set; }

	public string Uri { get; set; }

	public string Login { get; set; }

	public string Password { get; set; }

	public string SecretKey { get; set; }

	public string TextTemplate { get; set; }
}