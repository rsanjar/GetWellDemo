using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IPatientService : IBaseService<Patient>
    {
        Task<List<Patient>> GetAllAsync(PatientSearch search);

        Task<CrudResponse> Register(Patient patient);

        Task<Patient> GetProfile(int patientID);

        Task<CrudResponse> SaveProfile(Patient model);
    }
}