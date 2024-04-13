using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicPhoneService : BaseService<ClinicPhone, Data.Model.ClinicPhone>, IClinicPhoneService
	{
        #region ctor

        private readonly IRepository<Data.Model.ClinicPhone> _repository;

        public ClinicPhoneService(IRepository<Data.Model.ClinicPhone> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<ClinicPhone>> GetAllAsync(int clinicId)
        {
            var query = _repository.Context.ClinicPhones
                .Where(c => c.ClinicID == clinicId)
                .Select(c => c);

            var result = await ToListAsync(query);

            return result;
        }

        public async Task<List<ClinicPhone>> GetAllByServiceClinicDoctorAsync(int serviceClinicDoctorID)
        {
            var query = from c in _repository.Entity
                join k in _repository.Context.ServiceClinics on c.ClinicID equals k.ClinicID
                join f in _repository.Context.ServiceClinicDoctors on k.ID equals f.ServiceClinicID
                where f.ID == serviceClinicDoctorID
                select c;

            var result = await ToListAsync(query);

            return result;
        }
    }
}