using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class ClinicDoctorService : BaseService<ClinicDoctor, Data.Model.ClinicDoctor>, IClinicDoctorService
    {
        #region ctor

        private readonly IRepository<Data.Model.ClinicDoctor> _repository;

        public ClinicDoctorService(IRepository<Data.Model.ClinicDoctor> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public override async Task<ClinicDoctor> GetAsync(int id)
        {
	        var query = from c in _repository.Context.ClinicDoctors
		        let rating = (from a in _repository.Context.Appointments
			        join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
			        join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
					join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
			        select ap).Average(r => r.PatientRating)
		        join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
		        where c.ID == id
		        select new { c, rating, dp };

	        var result = await query.Select(c => new ClinicDoctor()
	        {
		        ID = c.c.ID,
		        ClinicID = c.c.ClinicID,
		        DoctorID = c.c.DoctorID,
		        DateCreated = c.c.DateCreated,
		        DateDisabled = c.c.DateDisabled,
		        IsActive = c.c.IsActive,
		        Rating = c.rating,
		        Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
		        DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()

	        }).FirstOrDefaultAsync();

	        return result;
        }

        public async Task<List<ClinicDoctor>> GetAllAsync(int clinicId)
        {
	        var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
		        join d in _repository.Context.Doctors on c.DoctorID equals d.ID
                join dp in _repository.Context.DoctorProfiles on d.ID equals dp.DoctorID
		        where c.ClinicID == clinicId &&
                      c.IsActive == true && d.IsActive == true

                select new { c, rating, dp };

			var result = await query.Select(c => new ClinicDoctor()
			{
				ID = c.c.ID,
				ClinicID = c.c.ClinicID,
				DoctorID = c.c.DoctorID,
				DateCreated = c.c.DateCreated,
				DateDisabled = c.c.DateDisabled,
				IsActive = c.c.IsActive,
				Rating = c.rating,
				Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
				DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
			}).Distinct().ToListAsync();

			return result;
        }

        public async Task<List<ClinicDoctor>> GetAllByServiceAsync(int serviceClinicId, int patientId = 0)
        {
	        var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
				let favorite = patientId > 0 && (from fav in _repository.Context.PatientFavoriteDoctors 
					where fav.ClinicDoctorID == c.ID && fav.PatientID == patientId
					select fav).Any()
		        join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ClinicDoctorID
                join sc in _repository.Context.ServiceClinics on k.ServiceClinicID equals sc.ID
                join d in _repository.Context.Doctors on c.DoctorID equals d.ID
                join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
		        where k.ServiceClinicID == serviceClinicId && sc.ClinicID == c.ClinicID && 
					  c.IsActive == true && k.IsActive == true && 
                      d.IsActive == true && sc.IsActive == true
		        select new { c, rating, dp, favorite, k };

	        var result = await query.Select(c => new ClinicDoctor()
	        {
		        ID = c.c.ID,
		        ClinicID = c.c.ClinicID,
		        DoctorID = c.c.DoctorID,
                ServiceClinicDoctorID = c.k.ID,
		        DateCreated = c.c.DateCreated,
		        DateDisabled = c.c.DateDisabled,
                IsActive = c.c.IsActive,
		        Rating = c.rating,
				IsPatientFavorite = c.favorite,
		        Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
				DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
	        }).Distinct().ToListAsync();

	        return result;
        }

        public async Task<List<ClinicDoctor>> GetAllByServiceBasicAsync(int serviceClinicId)
        {
            var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
                join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ClinicDoctorID
				join sc in _repository.Context.ServiceClinics on k.ServiceClinicID equals sc.ID
                join d in _repository.Context.Doctors on c.DoctorID equals d.ID
                join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
                where k.ServiceClinicID == serviceClinicId && c.IsActive == true && sc.ClinicID == c.ClinicID && 
                      k.IsActive == true && d.IsActive == true && sc.IsActive == true
                select new { c, d };

            var result = await query.Select(c => new ClinicDoctor()
            {
                ID = c.c.ID,
				Doctor = new Doctor()
                {
					FirstName = c.d.FirstName,
					LastName = c.d.LastName,
					MiddleName = c.d.MiddleName,
                    FirstNameEn = c.d.FirstNameEn,
                    LastNameEn = c.d.LastNameEn,
                    MiddleNameEn = c.d.MiddleNameEn,
                    FirstNameUz = c.d.FirstNameUz,
                    LastNameUz = c.d.LastNameUz,
                    MiddleNameUz = c.d.MiddleNameUz
                }

            }).Distinct().ToListAsync();

            return result;
        }

        public async Task<List<ClinicDoctor>> GetAllAsync(int clinicId, int serviceId)
        {
	        var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
		        join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ClinicDoctorID
		        join d in _repository.Context.Doctors on c.DoctorID equals d.ID
		        join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
		        join sc in _repository.Context.ServiceClinics on k.ServiceClinicID equals sc.ID
                join s in _repository.Context.Services on sc.ServiceID equals s.ID
		        where s.ID == serviceId && c.ClinicID == clinicId  && sc.ClinicID == c.ClinicID && 
                      k.IsActive == true && d.IsActive == true && sc.IsActive == true &&
					  c.IsActive == true && s.IsActive == true
		        select new { c, k, rating, dp };

	        var result = await query.Select(c => new ClinicDoctor()
	        {
		        ID = c.c.ID,
		        ClinicID = c.c.ClinicID,
		        DoctorID = c.c.DoctorID,
				ServiceClinicDoctorID = c.k.ID,
		        DateCreated = c.c.DateCreated,
		        DateDisabled = c.c.DateDisabled,
		        IsActive = c.c.IsActive,
		        Rating = c.rating,
		        Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
		        DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
	        }).Distinct().ToListAsync();

	        return result;
        }

        public async Task<List<ClinicDoctor>> GetAllByServiceCategoryAsync(int clinicId, int serviceCategoryId)
        {
	        var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
		        join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ClinicDoctorID
		        join d in _repository.Context.Doctors on c.DoctorID equals d.ID
		        join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
		        join sc in _repository.Context.ServiceClinics on k.ServiceClinicID equals sc.ID
		        join s in _repository.Context.Services on sc.ServiceID equals s.ID
				join cat in _repository.Context.ServiceCategories on s.ServiceCategoryID equals cat.ID
		        where cat.ID == serviceCategoryId && c.ClinicID == clinicId && sc.ClinicID == c.ClinicID && 
                      k.IsActive == true && d.IsActive == true && sc.IsActive == true
		        select new { c, rating, dp };

	        var result = await query.Select(c => new ClinicDoctor()
	        {
		        ID = c.c.ID,
		        ClinicID = c.c.ClinicID,
		        DoctorID = c.c.DoctorID,
		        DateCreated = c.c.DateCreated,
		        DateDisabled = c.c.DateDisabled,
		        IsActive = c.c.IsActive,
		        Rating = c.rating,
		        Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
		        DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
	        }).Distinct().ToListAsync();

	        return result;
        }

        public async Task<ClinicDoctor> GetAsync(int clinicId, int doctorId)
        {
            var query = from c in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == c.ID && c.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
	            join dp in _repository.Context.DoctorProfiles on c.DoctorID equals dp.DoctorID
	            where c.ClinicID == clinicId && c.DoctorID == doctorId
	            select new { c, rating, dp };

            var result = await query.Select(c => new ClinicDoctor()
            {
	            ID = c.c.ID,
	            ClinicID = c.c.ClinicID,
	            DoctorID = c.c.DoctorID,
	            DateCreated = c.c.DateCreated,
	            DateDisabled = c.c.DateDisabled,
	            IsActive = c.c.IsActive,
	            Rating = c.rating,
	            Doctor = c.c.Doctor.Map<Data.Model.Doctor, Doctor>(),
	            DoctorProfile = c.dp.Map<Data.Model.DoctorProfile, DoctorProfile>()
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<int?> GetClinicIDAsync(int clinicDoctorId)
        {
	        var query = await _repository.Entity
		        .FirstOrDefaultAsync(c => c.ID == clinicDoctorId);

	        return query?.ClinicID;
        }

        public async Task<int?> GetIDAsync(int serviceClinicDoctorID)
        {
	        var query = from c in _repository.Entity
		        join k in _repository.Context.ServiceClinics on c.ClinicID equals k.ClinicID
		        join cds in _repository.Context.ServiceClinicDoctors on k.ID equals cds.ServiceClinicID
		        where cds.ID == serviceClinicDoctorID
		        select c;

	        var result =  await FirstOrDefaultAsync(query);

	        return result?.ID;
        }
    }
}