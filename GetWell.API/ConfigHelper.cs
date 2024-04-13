using GetWell.API.JwtManager;
using GetWell.API.Models;
using Microsoft.Extensions.Configuration;

namespace GetWell.API;

public class ConfigHelper
{
	private readonly IConfiguration _configuration;

	public ConfigHelper(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string ConnectionString => _configuration.GetConnectionString("GetWellDatabase");

	public JwtTokenConfig JwtTokenConfig => _configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();

	public SmsConfig SmsConfig => _configuration.GetSection("SmsConfig").Get<SmsConfig>();
}