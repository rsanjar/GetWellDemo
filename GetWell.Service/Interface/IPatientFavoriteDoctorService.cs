using GetWell.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;

namespace GetWell.Service.Interface;

public interface IPatientFavoriteDoctorService : IBaseService<PatientFavoriteDoctor>
{
	Task<List<PatientFavoriteDoctor>> GetAllAsync(int patientID);

    Task<CrudResponse> AddAsync(int clinicDoctorID, int patientID);

	Task<CrudResponse> RemoveAsync(int clinicDoctorID, int patientID);
}