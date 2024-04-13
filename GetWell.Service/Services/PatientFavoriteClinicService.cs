using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Crud = GetWell.Core.Enums.Crud;
using CrudResponse = GetWell.Core.CrudResponse;

namespace GetWell.Service.Services;

public class PatientFavoriteClinicService : BaseService<PatientFavoriteClinic, Data.Model.PatientFavoriteClinic>, IPatientFavoriteClinicService
{
	#region ctor

	private readonly IRepository<Data.Model.PatientFavoriteClinic> _repository;

	public PatientFavoriteClinicService(IRepository<Data.Model.PatientFavoriteClinic> repository) : base(repository)
	{
		_repository = repository;
	}

	#endregion

	public async Task<List<PatientFavoriteClinic>> GetAllAsync(int patientID)
	{
		var query = from c in _repository.Entity
            join k in _repository.Context.Clinics on c.ClinicID equals k.ID
            let rating = (from a in _repository.Context.Appointments
                join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                where a.IsArchived && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                select ap).Average(r => r.PatientRating)
            where c.PatientID == patientID
			select new { c, k, rating };

		var result = await query.Select(c => new PatientFavoriteClinic
		{
			ID = c.c.ID,
			ClinicID = c.c.ClinicID,
			PatientID = c.c.PatientID,
			DateCreated = c.c.DateCreated,
			DateUpdated = c.c.DateUpdated,
			SortOrder = c.c.SortOrder,
			Rating = c.rating,
			Clinic = c.k.Map<Data.Model.Clinic, Clinic>()
        }).ToListAsync();

		return result;
	}

    public async Task<CrudResponse> AddAsync(int clinicID, int patientID)
    {
        bool any = _repository.Entity
            .Any(c => c.ClinicID == clinicID && c.PatientID == patientID);

        if (any)
            return new CrudResponse(Crud.DuplicateEntryError);

        await _repository.SaveAsync(new Data.Model.PatientFavoriteClinic()
        {
            PatientID = patientID,
            ClinicID = clinicID,
            SortOrder = 1
        });
			
        return new CrudResponse(Crud.Success);
    }
	
	public async Task<CrudResponse> RemoveAsync(int clinicID, int patientID)
	{
		var query = _repository.Entity
			.FirstOrDefault(c => c.ClinicID == clinicID && c.PatientID == patientID);

		if (query == null)
			return new CrudResponse(Crud.ItemNotFoundError);

        await _repository.DeleteAsync(query.ID);

		return new CrudResponse(Crud.Success);
	}
}