using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class ClinicDoctorAccountService : 
		AuthenticatableService<ClinicDoctorAccount, Data.Model.ClinicDoctorAccount>, IClinicDoctorAccountService
	{
        #region ctor

        private readonly IRepository<Data.Model.ClinicDoctorAccount> _repository;

        public ClinicDoctorAccountService(IRepository<Data.Model.ClinicDoctorAccount> repository) : base(repository)
        {
            _repository = repository;
        }


        #endregion

        public async Task<ClinicDoctorAccount> GetByClinicDoctorAsync(int clinicDoctorID)
        {
	        var result = await (from c in _repository.Entity 
			        join k in _repository.Context.ClinicDoctors on c.ClinicDoctorID equals k.ID 
			        where k.ID == clinicDoctorID
			        select c)
		        .FirstOrDefaultAsync<Data.Model.ClinicDoctorAccount, ClinicDoctorAccount>();

	        return result;
        }

        public async Task<ClinicDoctorAccount> GetByClinicDoctorAsync(int clinicID, int doctorID)
        {
            var result = await (from c in _repository.Entity 
                    join k in _repository.Context.ClinicDoctors on c.ClinicDoctorID equals k.ID 
                    where k.ClinicID == clinicID && k.DoctorID == doctorID
                    select c)
                .FirstOrDefaultAsync<Data.Model.ClinicDoctorAccount, ClinicDoctorAccount>();

            return result;
        }

        public async Task<int> GetDoctorIDAsync(int id)
        {
            int doctorID = 0;
            var result = await (from c in _repository.Entity
                    join k in _repository.Context.ClinicDoctors on c.ClinicDoctorID equals k.ID
                    where c.ID == id
                    select k).FirstOrDefaultAsync();

            if (result != null)
                doctorID = result.DoctorID;

            return doctorID;
        }
	}
}