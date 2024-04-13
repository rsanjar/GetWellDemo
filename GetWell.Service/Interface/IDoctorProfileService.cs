using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IDoctorProfileService : IBaseService<DTO.DoctorProfile>
    {
        Task<DoctorProfile> GetByDoctorAsync(int doctorID);
    }
}