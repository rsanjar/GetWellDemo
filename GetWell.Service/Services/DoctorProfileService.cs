using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class DoctorProfileService : BaseService<DoctorProfile, Data.Model.DoctorProfile>, IDoctorProfileService
    {
        #region ctor

        private readonly IRepository<Data.Model.DoctorProfile> _repository;

        public DoctorProfileService(IRepository<Data.Model.DoctorProfile> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<DoctorProfile> GetByDoctorAsync(int doctorID)
        {
            var result = await _repository.Entity
                .Where(c => c.DoctorID == doctorID)
                .FirstOrDefaultAsync<Data.Model.DoctorProfile, DoctorProfile>();

            return result;
        }
	}
}