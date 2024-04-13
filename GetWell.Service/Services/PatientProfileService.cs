using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Crud = GetWell.Core.Enums.Crud;

namespace GetWell.Service.Services
{
	public class PatientProfileService : BaseService<PatientProfile, Data.Model.PatientProfile>, IPatientProfileService
	{
        #region ctor

        private readonly IRepository<Data.Model.PatientProfile> _repository;

        public PatientProfileService(IRepository<Data.Model.PatientProfile> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<PatientProfile> GetByPatientAsync(int patientID)
        {
            var query = await _repository.Context.PatientProfiles
                .Where(c => c.PatientID == patientID)
                .FirstOrDefaultAsync<Data.Model.PatientProfile, PatientProfile>();

            return query;
        }

        public async Task<string> GetProfilePhotoBase64(int patientID)
        {
            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.PatientID == patientID);

            if (query != null)
                return query.PhotoBase64;

            return string.Empty;
        }

        public async Task<Core.CrudResponse> UpdateProfilePhoto(int patientID, string photoBase64)
        {
            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.PatientID == patientID);

            if (query == null)
                return new Core.CrudResponse(Crud.ItemNotFoundError);

            query.PhotoBase64 = photoBase64;

            await _repository.Context.SaveChangesAsync();

            return new Core.CrudResponse(Crud.Success);
        }
	}
}