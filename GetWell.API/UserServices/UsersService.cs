using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Service.Interface;
using Microsoft.Extensions.Logging;

namespace GetWell.API.UserServices
{
	public class UserService<T> : IUserService<T> where T : IAuthenticatable
    {
	    #region ctor

	    private readonly ILogger<UserService<T>> _logger;
	    private readonly T _service;

	    public UserService(ILogger<UserService<T>> logger, T service)
	    {
		    _logger = logger;
		    _service = service;
	    }

	    #endregion

	    public async Task<bool> IsValidUserCredentials(string userName, string password)
        {
	        _logger.LogInformation($"Validating user [{userName}]");

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            
            return await _service.IsValidUserCredentialsAsync(userName, password);
        }

        public async Task<bool> IsAnExistingUser(string userName)
        {
            return await _service.IsAnExistingUserAsync(userName);
        }

        public async Task<string> GetUserRole(string userName)
        {
            if (!await IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.Patient;
        }
    }
}
