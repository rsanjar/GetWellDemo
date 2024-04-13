using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
    public interface IServiceClinicDoctorService : IBaseService<ServiceClinicDoctor>
    {
        Task<List<ServiceClinicDoctor>> GetAllByDoctor(int clinicId, int doctorId, int? serviceCategoryId = null);
        
        Task<int> GetID(int serviceClinicID, int clinicDoctorID);

        Task<int> GetClinicID(int id);
    }
}