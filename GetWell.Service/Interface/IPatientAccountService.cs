using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IPatientAccountService : IBaseService<PatientAccount>, IAuthenticatable
    {
        Task<PatientAccount> GetByPatientIDAsync(int patientID);

        Task<bool> Disable(int patientID);
    }
}