using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class PatientAccountService : AuthenticatableService<PatientAccount, Data.Model.PatientAccount>, IPatientAccountService
	{
        #region ctor

        private readonly IRepository<Data.Model.PatientAccount> _repository;

        public PatientAccountService(IRepository<Data.Model.PatientAccount> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<PatientAccount> GetByPatientIDAsync(int patientID)
        {
            var query = await _repository.Entity
                .Where(c => c.PatientID == patientID)
                .FirstOrDefaultAsync<Data.Model.PatientAccount, PatientAccount>();

            return query;
        }

        public async Task<bool> Disable(int patientID)
        {
            var query = await _repository.Entity.FirstOrDefaultAsync(c => c.PatientID == patientID);

            if (query == null)
                return false;

            query.IsActive = false;
            query.IsPhoneVerified = false;

            await _repository.Context.SaveChangesAsync();

            return true;
        }
	}
}