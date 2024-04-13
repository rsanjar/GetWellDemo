using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class DoctorPhoneService : BaseService<DoctorPhone, Data.Model.DoctorPhone>, IDoctorPhoneService
    {
        #region ctor

        private readonly IRepository<Data.Model.DoctorPhone> _repository;

        public DoctorPhoneService(IRepository<Data.Model.DoctorPhone> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<DoctorPhone>> GetAllAsync(int doctorId)
        {
            var query = _repository.Context.DoctorPhones
                .Where(c => c.DoctorID == doctorId)
                .Select(c => c);

            var result = await ToListAsync(query);

            return result;
        }

        public async Task<List<DoctorPhone>> GetAllByServiceClinicDoctorAsync(int serviceClinicDoctorID)
        {
            var query = from c in _repository.Entity
                join k in _repository.Context.ClinicDoctors on c.DoctorID equals k.DoctorID
                join f in _repository.Context.ServiceClinicDoctors on k.ID equals f.ClinicDoctorID
                where f.ID == serviceClinicDoctorID
                select c;

            var result = await ToListAsync(query);

            return result;
        }


	}
}