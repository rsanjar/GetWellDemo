using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicDoctorAccountService : IBaseService<ClinicDoctorAccount>, IAuthenticatable
	{
		Task<ClinicDoctorAccount> GetByClinicDoctorAsync(int clinicDoctorID);

        Task<ClinicDoctorAccount> GetByClinicDoctorAsync(int clinicID, int doctorID);

        Task<int> GetDoctorIDAsync(int id);
    }
}