using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicDoctorWorkDayService : IBaseService<ClinicDoctorWorkDay>
    {
        Task<List<ClinicDoctorWorkDay>> GetAllAsync(int clinicDoctorID);

        Task<List<ClinicDoctorWorkDay>> GetAllAsync(int clinicID, int doctorID);

        Task<ClinicDoctorWorkDay> GetAsync(int clinicDoctorID, DateTime date);
    }
}