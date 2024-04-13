using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class AdminAccountService : AuthenticatableService<AdminAccount, Data.Model.AdminAccount>, IAdminAccountService
	{
		public AdminAccountService(IRepository<Data.Model.AdminAccount> repository) : base(repository)
		{
		}
	}
}