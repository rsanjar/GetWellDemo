using GetWell.DTO;
using GetWell.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using Microsoft.EntityFrameworkCore;
using CrudResponse = GetWell.Core.CrudResponse;
using Crud = GetWell.Core.Enums.Crud;

namespace GetWell.Service.Services;

public class PatientFavoriteDoctorService : BaseService<PatientFavoriteDoctor, Data.Model.PatientFavoriteDoctor>, IPatientFavoriteDoctorService
{
	#region ctor

	private readonly IRepository<Data.Model.PatientFavoriteDoctor> _repository;

	public PatientFavoriteDoctorService(IRepository<Data.Model.PatientFavoriteDoctor> repository) : base(repository)
	{
		_repository = repository;
	}

	#endregion

	public async Task<List<PatientFavoriteDoctor>> GetAllAsync(int patientID)
	{
		var query = from c in _repository.Entity 
            join k in _repository.Context.ClinicDoctors on c.ClinicDoctorID equals k.ID
            join clinic in _repository.Context.Clinics on k.ClinicID equals clinic.ID
            let rating = (from a in _repository.Context.Appointments
                join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                where a.IsArchived && scd.ClinicDoctorID == k.ID && k.ClinicID == sc.ClinicID && ap.PatientRating != null
                select ap).Average(r => r.PatientRating)
			join d in _repository.Context.Doctors on k.DoctorID equals d.ID
			join dp in _repository.Context.DoctorProfiles on d.ID equals dp.DoctorID
			where c.PatientID == patientID
			select new { c, k, d, dp, clinic, rating };

		var result = await query.Select(c => new PatientFavoriteDoctor
		{
			ID = c.c.ID,
			ClinicDoctorID = c.c.ClinicDoctorID,
			PatientID = c.c.PatientID,
			DateCreated = c.c.DateCreated,
			DateUpdated = c.c.DateUpdated,
			SortOrder = c.c.SortOrder,
			Rating = c.rating,
			Doctor = c.d.Map<Data.Model.Doctor, Doctor>(),
			DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
            
        }).ToListAsync();

		return result;
	}

    public async Task<CrudResponse> AddAsync(int clinicDoctorID, int patientID)
    {
        bool any = _repository.Entity
            .Any(c => c.ClinicDoctorID == clinicDoctorID && c.PatientID == patientID);

        if (any)
            return new CrudResponse(Crud.DuplicateEntryError);

        await _repository.SaveAsync(new Data.Model.PatientFavoriteDoctor()
        {
            PatientID = patientID,
            ClinicDoctorID = clinicDoctorID,
			SortOrder = 1
        });
			
        return new CrudResponse(Crud.Success);
    }

	public async Task<CrudResponse> RemoveAsync(int clinicDoctorID, int patientID)
	{
		var query = _repository.Entity
			.FirstOrDefault(c => c.ClinicDoctorID == clinicDoctorID && c.PatientID == patientID);

		if (query == null)
			return new CrudResponse(Crud.ItemNotFoundError);

        await _repository.DeleteAsync(query.ID);
		
		return new CrudResponse(Crud.Success);
	}
}