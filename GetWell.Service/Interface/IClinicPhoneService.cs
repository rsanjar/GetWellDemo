using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicPhoneService : IBaseService<ClinicPhone>
    {
        Task<List<ClinicPhone>> GetAllAsync(int clinicId);

        Task<List<ClinicPhone>> GetAllByServiceClinicDoctorAsync(int serviceClinicDoctorID);
    }
}