using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicRoleService : BaseService<ClinicRole, Data.Model.ClinicRole>, IClinicRoleService
	{
		public ClinicRoleService(IRepository<Data.Model.ClinicRole> repository) : base(repository)
		{
		}
	}
}