using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IDoctorPhoneService : IBaseService<DTO.DoctorPhone>
    {
        Task<List<DoctorPhone>> GetAllAsync(int doctorId);

        Task<List<DoctorPhone>> GetAllByServiceClinicDoctorAsync(int serviceClinicDoctorID);
    }
}