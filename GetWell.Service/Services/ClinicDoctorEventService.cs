using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services;

public class ClinicDoctorEventService : BaseService<ClinicDoctorEvent, Data.Model.ClinicDoctorEvent>, IClinicDoctorEventService
{
    #region ctor

    private readonly IRepository<Data.Model.ClinicDoctorEvent> _repository;

    public ClinicDoctorEventService(IRepository<Data.Model.ClinicDoctorEvent> repository) : base(repository)
    {
        _repository = repository;
    }

    #endregion

    public async Task<List<ClinicDoctorEvent>> GetAllByDoctorDayAsync(int clinicDoctorID, DateTime eventDate)
    {
        DateTime date = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day);

        return await GetAllByClinicDoctorAsync(clinicDoctorID, date, date);
    }

    public async Task<List<ClinicDoctorEvent>> GetAllByDoctorWeekAsync(int clinicDoctorID, int year, int weekOfYear)
    {
        DateTime dateStart = ISOWeek.ToDateTime(year, weekOfYear, DayOfWeek.Monday);
        DateTime dateEnd = ISOWeek.ToDateTime(year, weekOfYear, DayOfWeek.Sunday);

        return await GetAllByClinicDoctorAsync(clinicDoctorID, dateStart, dateEnd);
    }
    
    public async Task<List<ClinicDoctorEvent>> GetAllByDoctorMonthAsync(int clinicDoctorID, int year, int month)
    {
        DateTime dateStart = new DateTime(year, month, 1);
        DateTime dateEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        
        return await GetAllByClinicDoctorAsync(clinicDoctorID, dateStart, dateEnd);
    }

    public async Task<List<ClinicDoctorEvent>> GetAllByClinicDayAsync(int clinicID, DateTime eventDate)
    {
        DateTime date = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day);

        return await GetAllByClinicAsync(clinicID, date, date);
    }

    public async Task<List<ClinicDoctorEvent>> GetAllByClinicWeekAsync(int clinicID, int year, int weekOfYear)
    {
        DateTime dateStart = ISOWeek.ToDateTime(year, weekOfYear, DayOfWeek.Monday);
        DateTime dateEnd = ISOWeek.ToDateTime(year, weekOfYear, DayOfWeek.Sunday);

        return await GetAllByClinicAsync(clinicID, dateStart, dateEnd);
    }
    
    public async Task<List<ClinicDoctorEvent>> GetAllByClinicMonthAsync(int clinicID, int year, int month)
    {
        DateTime dateStart = new DateTime(year, month, 1);
        DateTime dateEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        
        return await GetAllByClinicAsync(clinicID, dateStart, dateEnd);
    }

    public async Task<List<ClinicDoctorEvent>> GetAllByClinicAsync(int clinicID, DateTime dateStart, DateTime dateEnd)
    {
        var query = from a in _repository.Context.ClinicDoctorEvents
            join cd in _repository.Context.ClinicDoctors on a.ClinicDoctorID equals cd.ID
            where cd.ClinicID == clinicID && 
                  a.AppointmentStartDate <= dateStart && a.AppointmentStartDate >= dateEnd 
                  && a.IsCompleted == false && a.IsCanceled == false
            orderby a.AppointmentStartDate, a.AppointmentStartTime
            select a;

        var result = await ToListAsync(query);

        return result;
    }

    public async Task<List<ClinicDoctorEvent>> GetAllByClinicDoctorAsync(int clinicDoctorID, DateTime dateStart, DateTime dateEnd)
    {
        var query = from a in _repository.Context.ClinicDoctorEvents
            where a.ClinicDoctorID == clinicDoctorID && 
                  a.AppointmentStartDate <= dateStart && a.AppointmentStartDate >= dateEnd 
                  && a.IsCompleted == false && a.IsCanceled == false
            orderby a.AppointmentStartDate, a.AppointmentStartTime
            select a;

        var result = await ToListAsync(query);

        return result;
    }
}