using System.Threading.Tasks;
using GetWell.Core;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IPatientProfileService : IBaseService<PatientProfile>
    {
        Task<PatientProfile> GetByPatientAsync(int patientID);

        Task<string> GetProfilePhotoBase64(int patientID);

        Task<CrudResponse> UpdateProfilePhoto(int patientID, string photoBase64);
    }
}