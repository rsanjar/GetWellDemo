using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicDoctorService : IBaseService<ClinicDoctor>
    {
        Task<List<ClinicDoctor>> GetAllAsync(int clinicId);

        Task<ClinicDoctor> GetAsync(int clinicId, int doctorId);

        Task<List<ClinicDoctor>> GetAllByServiceAsync(int serviceClinicId, int patientId = 0);

        Task<List<ClinicDoctor>> GetAllByServiceBasicAsync(int serviceClinicId);

        Task<List<ClinicDoctor>> GetAllAsync(int clinicId, int serviceId);

        Task<List<ClinicDoctor>> GetAllByServiceCategoryAsync(int clinicId, int serviceCategoryId);

        Task<int?> GetClinicIDAsync(int clinicDoctorId);

        Task<int?> GetIDAsync(int serviceClinicDoctorID);
    }
}