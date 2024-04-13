using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface;

public interface IClinicDoctorEventService : IBaseService<ClinicDoctorEvent>
{
    Task<List<ClinicDoctorEvent>> GetAllByDoctorDayAsync(int clinicDoctorID, DateTime eventDate);
    Task<List<ClinicDoctorEvent>> GetAllByDoctorWeekAsync(int clinicDoctorID, int year, int weekOfYear);
    Task<List<ClinicDoctorEvent>> GetAllByDoctorMonthAsync(int clinicDoctorID, int year, int month);
    Task<List<ClinicDoctorEvent>> GetAllByClinicDayAsync(int clinicID, DateTime eventDate);
    Task<List<ClinicDoctorEvent>> GetAllByClinicWeekAsync(int clinicID, int year, int weekOfYear);
    Task<List<ClinicDoctorEvent>> GetAllByClinicMonthAsync(int clinicID, int year, int month);
    Task<List<ClinicDoctorEvent>> GetAllByClinicAsync(int clinicID, DateTime dateStart, DateTime dateEnd);
    Task<List<ClinicDoctorEvent>> GetAllByClinicDoctorAsync(int clinicDoctorID, DateTime dateStart, DateTime dateEnd);
}