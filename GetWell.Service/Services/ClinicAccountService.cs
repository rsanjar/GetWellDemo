using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicAccountService : AuthenticatableService<ClinicAccount, Data.Model.ClinicAccount>, IClinicAccountService
	{
        #region ctor

        private readonly IRepository<Data.Model.ClinicAccount> _repository;

        public ClinicAccountService(IRepository<Data.Model.ClinicAccount> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<ClinicAccount> GetByClinicAsync(int clinicId)
        {
            var query = _repository.Context.ClinicAccounts
                .Where(c => c.ClinicID == clinicId);

            var result = await FirstOrDefaultAsync(query);

            return result;
        }
    }
}