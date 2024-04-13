using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.DTO;

namespace GetWell.Service.Interface;

public interface IPatientFavoriteClinicService : IBaseService<PatientFavoriteClinic>
{
	Task<List<PatientFavoriteClinic>> GetAllAsync(int patientID);

    Task<CrudResponse> AddAsync(int clinicID, int patientID);

	Task<CrudResponse> RemoveAsync(int clinicID, int patientID);
}