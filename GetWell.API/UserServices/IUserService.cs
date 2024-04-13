using System.Threading.Tasks;
using GetWell.Service.Interface;

namespace GetWell.API.UserServices
{
	public interface IUserService<T> where T : IAuthenticatable
	{
		Task<bool> IsAnExistingUser(string userName);
		Task<bool> IsValidUserCredentials(string userName, string password);
		Task<string> GetUserRole(string userName);
	}
}