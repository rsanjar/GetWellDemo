using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class AppointmentService : BaseService<Appointment, Data.Model.Appointment>, IAppointmentService
	{
		#region ctor

		private readonly Data.IRepository<Data.Model.Appointment> _repository;

		public AppointmentService(Data.IRepository<Data.Model.Appointment> repository) : base(repository)
		{
			_repository = repository;
		}

		#endregion

		public async Task<List<Appointment>> GetAllAsync(AppointmentSearch search)
		{
			var query = from patient in _repository.Context.Patients
				join patientProfile in _repository.Context.PatientProfiles on patient.ID equals patientProfile.PatientID
				join appointment in _repository.Context.Appointments on patient.ID equals appointment.PatientID
				join appointmentProfile in _repository.Context.AppointmentProfiles on appointment.ID equals appointmentProfile.AppointmentID
				join serviceClinicDoctor in _repository.Context.ServiceClinicDoctors on appointment.ServiceClinicDoctorID equals serviceClinicDoctor.ID
				join clinicDoctor in _repository.Context.ClinicDoctors on serviceClinicDoctor.ClinicDoctorID equals clinicDoctor.ID
				join serviceClinic in _repository.Context.ServiceClinics on serviceClinicDoctor.ServiceClinicID equals serviceClinic.ID
				join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
				join clinic in _repository.Context.Clinics on serviceClinic.ClinicID equals clinic.ID
				join doctor in _repository.Context.Doctors on clinicDoctor.DoctorID equals doctor.ID
                join doctorProfile in _repository.Context.DoctorProfiles on doctor.ID equals doctorProfile.DoctorID
				join r in _repository.Context.Regions on clinic.RegionID equals r.ID
				where clinicDoctor.ClinicID == serviceClinic.ClinicID
				select new
				{
					patient, appointment, clinic, doctor, clinicDoctor, patientProfile, service, serviceClinicDoctor, serviceClinic, doctorProfile
				};

			if (search.ClinicDoctorID > 0)
				query = query.Where(c => c.clinicDoctor.ID == search.ClinicDoctorID);
			else if (search.ClinicID > 0)
				query = query.Where(c => c.clinic.ID == search.ClinicID);
			else if (!string.IsNullOrWhiteSpace(search.ClinicName))
				query = query.Where(c => EF.Functions.Like(c.clinic.Name, $"%{search.ClinicName}%"));
			else if (search.RegionID > 0)
				query = query.Where(c => c.clinic.RegionID == search.RegionID);
			else if (search.CityID > 0)
				query = query.Where(c => c.clinic.Region.CityID == search.CityID);
			
			if (search.AppointmentDateStart.HasValue)
				query = query.Where(c => c.appointment.AppointmentDate >= search.AppointmentDateStart.Value);

			if (search.AppointmentDateEnd.HasValue)
				query = query.Where(c => c.appointment.AppointmentDate <= search.AppointmentDateEnd.Value);

			if (search.IsActive.HasValue)
			{
				query = search.IsActive.Value
					? query.Where(c => c.appointment.AppointmentEndDate != null)
					: query.Where(c => c.appointment.AppointmentEndDate == null);
			}

			if (search.IsCanceled.HasValue)
				query = query.Where(c => c.appointment.IsCanceled == search.IsCanceled);

			if (search.IsDoctorConfirmed.HasValue)
				query = query.Where(c => c.appointment.IsDoctorConfirmed == search.IsDoctorConfirmed);

			var result = await query.Select(c => new Appointment
			{
				ID = c.appointment.ID,
				ClinicID = c.clinic.ID,
				DoctorID = c.doctor.ID,
                DoctorProfilePictureUrl = c.doctorProfile.ProfilePictureUrl,
				DateCreated = c.appointment.DateCreated,
				AppointmentDate = c.appointment.AppointmentDate,
				AppointmentTime = c.appointment.AppointmentTime,
				PatientName = c.patient.FirstName + " " + c.patient.LastName + " " + c.patient.MiddleName, //EF doesn't work with string.Format
				AppointmentEndDate = c.appointment.AppointmentEndDate,
				DiscountPercentage = c.appointment.DiscountPercentage,
				IsCanceled = c.appointment.IsCanceled,
				IsDoctorConfirmed = c.appointment.IsDoctorConfirmed,
				IsRefunded = c.appointment.IsRefunded,
				ServiceName = c.service.Name,
				Clinic = c.clinic.Map<Data.Model.Clinic, Clinic>(),
				Patient = c.patient.Map<Data.Model.Patient, Patient>(),
				PatientProfile = c.patientProfile.Map<Data.Model.PatientProfile, PatientProfile>(),
				Doctor = c.doctor.Map<Data.Model.Doctor, Doctor>(),
				ServiceClinicDoctor = c.serviceClinicDoctor.Map<Data.Model.ServiceClinicDoctor, ServiceClinicDoctor>(),
				AppointmentProfile = c.appointment.AppointmentProfile.Map<Data.Model.AppointmentProfile, AppointmentProfile>(),
				ServiceCategoryID = c.service.ServiceCategoryID,
				ServiceClinicID = c.serviceClinicDoctor.ServiceClinicID,
				ServiceClinicDoctorID = c.serviceClinicDoctor.ID,
				ClinicDoctorID = c.serviceClinicDoctor.ClinicDoctorID,
				PatientID = c.patient.ID,
				DoctorConfirmed = c.appointment.IsDoctorConfirmed ?? false,
				PriceBeforeDiscount = c.appointment.PriceBeforeDiscount,
				IsArchived = c.appointment.IsArchived,
				AverageDuration = c.serviceClinic.AverageDuration,
				IsPaid = c.appointment.IsPaid

			}).GetPaginatedListAsync(search);

			return result;
		}

		public async Task<PaginatedList<Appointment>> GetAllAsync(int patientID, PaginationModel<Appointment> pagination)
		{
			var query = from patient in _repository.Context.Patients
				join patientProfile in _repository.Context.PatientProfiles on patient.ID equals patientProfile.PatientID
				join appointment in _repository.Context.Appointments on patient.ID equals appointment.PatientID
				join appointmentProfile in _repository.Context.AppointmentProfiles on appointment.ID equals appointmentProfile.AppointmentID
				join serviceClinicDoctor in _repository.Context.ServiceClinicDoctors on appointment.ServiceClinicDoctorID equals serviceClinicDoctor.ID
				join clinicDoctor in _repository.Context.ClinicDoctors on serviceClinicDoctor.ClinicDoctorID equals clinicDoctor.ID
				join serviceClinic in _repository.Context.ServiceClinics on serviceClinicDoctor.ServiceClinicID equals serviceClinic.ID
				join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
				join clinic in _repository.Context.Clinics on serviceClinic.ClinicID equals clinic.ID
				join doctor in _repository.Context.Doctors on clinicDoctor.DoctorID equals doctor.ID
				join doctorProfile in _repository.Context.DoctorProfiles on doctor.ID equals doctorProfile.DoctorID
				join r in _repository.Context.Regions on clinic.RegionID equals r.ID
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ClinicDoctorID == clinicDoctor.ID && clinicDoctor.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(appointmentProfile => appointmentProfile.PatientRating)
				where appointment.PatientID == patientID && clinicDoctor.ClinicID == serviceClinic.ClinicID
				select new
				{
					patient, appointment, clinic, doctor, clinicDoctor, patientProfile, service, serviceClinicDoctor, serviceClinic, rating, doctorProfile
				};

			var result = await query.Select(c => new Appointment
			{
				ID = c.appointment.ID,
				ClinicID = c.clinic.ID,
				DoctorID = c.doctor.ID,
				DoctorProfilePictureUrl = c.doctorProfile.ProfilePictureUrl,
				DateCreated = c.appointment.DateCreated,
				AppointmentDate = c.appointment.AppointmentDate,
				AppointmentTime = c.appointment.AppointmentTime,
				PatientName = c.patient.FirstName + " " + c.patient.LastName + " " + c.patient.MiddleName, //EF doesn't work with string.Format
				AppointmentEndDate = c.appointment.AppointmentEndDate,
				DiscountPercentage = c.appointment.DiscountPercentage,
				IsCanceled = c.appointment.IsCanceled,
				IsDoctorConfirmed = c.appointment.IsDoctorConfirmed,
				IsRefunded = c.appointment.IsRefunded,
				ServiceName = c.service.Name,
				Clinic = c.clinic.Map<Data.Model.Clinic, Clinic>(),
				Patient = c.patient.Map<Data.Model.Patient, Patient>(),
				PatientProfile = c.patientProfile.Map<Data.Model.PatientProfile, PatientProfile>(),
				Doctor = c.doctor.Map<Data.Model.Doctor, Doctor>(),
				ServiceClinicDoctor = c.serviceClinicDoctor.Map<Data.Model.ServiceClinicDoctor, ServiceClinicDoctor>(),
				AppointmentProfile = c.appointment.AppointmentProfile.Map<Data.Model.AppointmentProfile, AppointmentProfile>(),
				ServiceCategoryID = c.service.ServiceCategoryID,
				ServiceClinicID = c.serviceClinicDoctor.ServiceClinicID,
				ServiceClinicDoctorID = c.serviceClinicDoctor.ID,
				ClinicDoctorID = c.serviceClinicDoctor.ClinicDoctorID,
				PatientID = c.patient.ID,
				DoctorConfirmed = c.appointment.IsDoctorConfirmed ?? false,
				PriceBeforeDiscount = c.appointment.PriceBeforeDiscount,
				IsArchived = c.appointment.IsArchived,
				AverageDuration = c.serviceClinic.AverageDuration,
                IsPaid = c.appointment.IsPaid

			}).GetPaginatedListAsync(pagination);

			return result;
		}

		public async Task<Appointment> GetDetailsAsync(int id)
        {
            var query = from patient in _repository.Context.Patients
                join patientProfile in _repository.Context.PatientProfiles on patient.ID equals patientProfile.PatientID
                join appointment in _repository.Context.Appointments on patient.ID equals appointment.PatientID
                join appointmentProfile in _repository.Context.AppointmentProfiles on appointment.ID equals appointmentProfile.AppointmentID
                join serviceClinicDoctor in _repository.Context.ServiceClinicDoctors on appointment.ServiceClinicDoctorID equals serviceClinicDoctor.ID
                join clinicDoctor in _repository.Context.ClinicDoctors on serviceClinicDoctor.ClinicDoctorID equals clinicDoctor.ID
                join serviceClinic in _repository.Context.ServiceClinics on serviceClinicDoctor.ServiceClinicID equals serviceClinic.ID
                join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
                join clinic in _repository.Context.Clinics on serviceClinic.ClinicID equals clinic.ID
                join doctor in _repository.Context.Doctors on clinicDoctor.DoctorID equals doctor.ID
                join doctorProfile in _repository.Context.DoctorProfiles on doctor.ID equals doctorProfile.DoctorID
                join r in _repository.Context.Regions on clinic.RegionID equals r.ID
                where appointment.ID == id && clinicDoctor.ClinicID == serviceClinic.ClinicID
                select new
                {
	                patient, appointment, clinic, doctor, clinicDoctor, patientProfile, service, serviceClinicDoctor, serviceClinic, doctorProfile
                };

            var result = await query.Select(c => new Appointment
            {
	            ID = c.appointment.ID,
	            ClinicID = c.clinic.ID,
	            DoctorID = c.doctor.ID,
                DoctorProfilePictureUrl = c.doctorProfile.ProfilePictureUrl,
	            DateCreated = c.appointment.DateCreated,
	            AppointmentDate = c.appointment.AppointmentDate,
	            AppointmentTime = c.appointment.AppointmentTime,
	            PatientName = c.patient.FirstName + " " + c.patient.LastName + " " + c.patient.MiddleName, //EF doesn't work with string.Format
	            AppointmentEndDate = c.appointment.AppointmentEndDate,
	            DiscountPercentage = c.appointment.DiscountPercentage,
	            IsCanceled = c.appointment.IsCanceled,
	            IsDoctorConfirmed = c.appointment.IsDoctorConfirmed,
	            IsRefunded = c.appointment.IsRefunded,
	            ServiceName = c.service.Name,
	            Clinic = c.clinic.Map<Data.Model.Clinic, Clinic>(),
	            Patient = c.patient.Map<Data.Model.Patient, Patient>(),
	            PatientProfile = c.patientProfile.Map<Data.Model.PatientProfile, PatientProfile>(),
	            Doctor = c.doctor.Map<Data.Model.Doctor, Doctor>(),
	            ServiceClinicDoctor = c.serviceClinicDoctor.Map<Data.Model.ServiceClinicDoctor, ServiceClinicDoctor>(),
				AppointmentProfile = c.appointment.AppointmentProfile.Map<Data.Model.AppointmentProfile, AppointmentProfile>(),
				ServiceCategoryID = c.service.ServiceCategoryID,
				ServiceClinicID = c.serviceClinicDoctor.ServiceClinicID,
				ServiceClinicDoctorID = c.serviceClinicDoctor.ID,
				ClinicDoctorID = c.serviceClinicDoctor.ClinicDoctorID,
				PatientID = c.patient.ID,
				DoctorConfirmed = c.appointment.IsDoctorConfirmed ?? false,
				PriceBeforeDiscount = c.appointment.PriceBeforeDiscount,
				AverageDuration = c.serviceClinic.AverageDuration,
				IsArchived = c.appointment.IsArchived,
				IsPaid = c.appointment.IsPaid
            }).FirstOrDefaultAsync();
			
            return result;
        }

		public async Task<List<Appointment>> GetAllAsync(int clinicDoctorID, DateTime appointmentDate)
		{
			DateTime date = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day);
            var query = from a in _repository.Context.Appointments
                join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                where scd.ClinicDoctorID == clinicDoctorID && a.AppointmentDate == date && cd.ClinicID == sc.ClinicID && a.IsArchived == false
                orderby appointmentDate, a.AppointmentTime
                select new Appointment
                {
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    AverageDuration = sc.AverageDuration
                };

			var result = await query.ToListAsync();

			return result;
		}

        public async Task<int> GetCountByClinic(int clinicID)
        {
            var query = from a in _repository.Context.Appointments
                join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                where cd.ClinicID == sc.ClinicID && cd.ClinicID == clinicID && a.IsDoctorConfirmed == false
                select a;
			
            var result = await query.CountAsync();

            return result;
        }

        public async Task<int> GetCountByClinicDoctor(int clinicDoctorID)
        {
            var query = from a in _repository.Context.Appointments
                join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                where cd.ClinicID == sc.ClinicID && cd.ID == clinicDoctorID && a.IsDoctorConfirmed == false
                select a;
			
            var result = await query.CountAsync();

            return result;
        }

		public async Task<CrudResponse> AddReviewAsync(Appointment model)
		{
			var query = await (from profile in _repository.Context.AppointmentProfiles
				join appointment in _repository.Context.Appointments on profile.AppointmentID equals appointment.ID
				where appointment.PatientID == model.PatientID && appointment.ID == model.ID && appointment.IsArchived == true
				select profile).FirstOrDefaultAsync();

			if (query == null)
				return new CrudResponse(Crud.ItemNotFoundError);

			query.PatientRating = model.AppointmentProfile.PatientRating;
			query.PatientReview = model.AppointmentProfile.PatientReview;

			await _repository.Context.SaveChangesAsync();

			return new CrudResponse(Crud.Success);
		}

        public async Task<string> GetQrImageBase64Async(int id, int patientID)
        {
            string result = string.Empty;

            var query = await (from c in _repository.Context.Appointments
                    join p in _repository.Context.AppointmentProfiles on c.ID equals p.AppointmentID
					where c.ID == id && c.PatientID == patientID
                    select p).FirstOrDefaultAsync();

            if (query != null)
                result = query.QrCodeBase64;
			
            return result;
        }

        public async Task<CrudResponse> CancelAppointmentAsync(int id, int patientID)
        {
            var query = await _repository
                .Entity
                .FirstOrDefaultAsync(c => c.ID == id && c.PatientID == patientID && 
                                          c.IsCanceled == false && c.IsArchived == false);

            if (query == null)
                return new CrudResponse(Crud.ItemNotFoundError);

            query.IsCanceled = true;

            await _repository.Context.SaveChangesAsync();

            return new CrudResponse(Crud.Success);
        }

        public async Task<CrudResponse> SetArchived(Guid code)
        {
            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.ConfirmationCode == code);

			if(query == null)
				return new CrudResponse(Crud.ItemNotFoundError);

            query.IsArchived = true;

            await _repository.Context.SaveChangesAsync();

            return new CrudResponse(Crud.Success);
        }

        public async Task<int> GetID(Guid code)
        {
            var query = await _repository.Entity.FirstOrDefaultAsync(c => c.ConfirmationCode == code);

            if(query != null)
                return query.ID;

            return 0;
        }
	}
}
	