using System.Threading.Tasks;
using GetWell.Data;

namespace GetWell.Service.Interface
{
	public interface IAuthenticatable
	{
		Task<bool> IsValidUserCredentialsAsync(string username, string password);

		Task<bool> IsValidUserCredentialsAsync(string phoneNumber, int smsActivationCode);

		Task<int> GetSmsActivationCode(string phoneNumber);

		Task<bool> IsAnExistingUserAsync(string username);

		Task<int> GetAccountID(string username, string password);

		Task<int> GetAccountID(string username);

		Task<bool> SetPhoneVerified(string username, bool isVerified = true);

		Task<bool> SetEmailVerified(string email);

		Task<bool> UpdatePassword(string username, string oldPassword, string newPassword);

		Task<bool> ResetPassword(string username, string resetPassword);

		Task<bool> SetActive(string username, bool isActive);

		Task<bool> UpdateLastLoginDate(string username);

        Task<string> GetPassword(string username);

        Task<string> GetEmail(string username);
    }
}