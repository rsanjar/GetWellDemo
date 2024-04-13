using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.Data.Model;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Crud = GetWell.Core.Enums.Crud;
using CrudResponse = GetWell.Core.CrudResponse;
using Patient = GetWell.DTO.Patient;
using PatientProfile = GetWell.DTO.PatientProfile;


namespace GetWell.Service.Services
{
	public class PatientService : BaseService<Patient, Data.Model.Patient>, IPatientService
	{
        #region ctor

        private readonly IRepository<Data.Model.Patient> _repository;
        private readonly IRepository<Data.Model.PatientAccount> _accountRepository;
        private readonly IRepository<Data.Model.PatientProfile> _profileRepository;

        public PatientService(IRepository<Data.Model.Patient> repository, 
	        IRepository<Data.Model.PatientAccount> accountRepository, 
            IRepository<Data.Model.PatientProfile> profileRepository) : base(repository)
        {
            _repository = repository;
            _accountRepository = accountRepository;
            _profileRepository = profileRepository;
        }

        #endregion

        public async Task<List<Patient>> GetAllAsync(PatientSearch search)
        {
            var query = from c in _repository.Context.Patients
                join p in _repository.Context.PatientProfiles on c.ID equals p.PatientID
                join k in _repository.Context.Appointments on c.ID equals k.PatientID
                join s in _repository.Context.ServiceClinicDoctors on k.ServiceClinicDoctorID equals s.ID
                join f in _repository.Context.ClinicDoctors on s.ClinicDoctorID equals f.ID
                join l in _repository.Context.ServiceClinics on s.ServiceClinicID equals l.ID
                join a in _repository.Context.Clinics on l.ClinicID equals a.ID
                join d in _repository.Context.Doctors on f.DoctorID equals d.ID
                join r in _repository.Context.Regions on a.RegionID equals r.ID
                where f.ClinicID == l.ClinicID
                select new { c, k, a, d, p };

            if (search.ClinicID > 0)
                query = query.Where(c => c.a.ID == search.ClinicID);
            else
            {
                if (search.CityID > 0)
                    query = query.Where(c => c.a.Region.CityID == search.CityID);

                if (search.RegionID > 0)
                    query = query.Where(c => c.a.RegionID == search.RegionID);
            }

            if (!string.IsNullOrWhiteSpace(search.FirstName))
                query = query.Where(c => EF.Functions.Like(c.c.FirstName, $"%{search.FirstName}%"));

            if (!string.IsNullOrWhiteSpace(search.LastName))
                query = query.Where(c => EF.Functions.Like(c.c.LastName, $"%{search.LastName}%"));

            if (!string.IsNullOrWhiteSpace(search.MiddleName))
                query = query.Where(c => EF.Functions.Like(c.c.MiddleName, $"%{search.MiddleName}%"));

            if (search.DateOfBirthFrom.HasValue)
                query = query.Where(c => c.p.DateOfBirth >= search.DateOfBirthFrom.Value);

            if (search.DateOfBirthTo.HasValue)
                query = query.Where(c => c.p.DateOfBirth <= search.DateOfBirthTo.Value);

            var result = await query.Select(c => new Patient
            {
                ID = c.c.ID,
                ClinicID = c.a.ID,
                ClinicName = c.a.Name,
                DoctorID = c.d.ID,
                IsActive = c.c.IsActive,
                FirstName = c.c.FirstName,
                LastName = c.c.LastName,
                MiddleName = c.c.MiddleName,
                PreferredLanguage = c.c.PreferredLanguage,
                Phone = c.c.Phone,
                DateCreated = c.c.DateCreated,
                PatientProfile = new PatientProfile
                {
                    DateOfBirth = c.p.DateOfBirth
                }
            }).GetPaginatedListAsync(search);

            return result;
        }

        public async Task<CrudResponse> Register(Patient patient)
        {
	        if (string.IsNullOrWhiteSpace(patient.FirstName) ||
	            string.IsNullOrWhiteSpace(patient.LastName) ||
	            string.IsNullOrWhiteSpace(patient.PatientAccount.MobilePhone)
	           )
	        {
		        return new CrudResponse(Crud.ValidationError);
	        }

	        if (string.IsNullOrWhiteSpace(patient.PatientAccount.Password))
		        patient.PatientAccount.Password = Guid.NewGuid().ToString();

	        bool isExists = _repository.Context.PatientAccounts
                .Any(c => c.MobilePhone == patient.PatientAccount.MobilePhone);

	        if (isExists)
		        return new CrudResponse(Crud.DuplicateEntryError);

	        var newPatient = new Data.Model.Patient()
	        {
		        FirstName = patient.FirstName,
		        LastName = patient.LastName,
                MiddleName = patient.MiddleName,
		        Phone = patient.PatientAccount.MobilePhone,
                Email = patient.Email,
		        PreferredLanguage = patient.PreferredLanguage,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                CityID = patient.CityID
            };

	        var result = await _repository.SaveAsync(newPatient);

	        if (result.MessageKey != Data.Crud.Success)
		        return new CrudResponse(Crud.Error);

	        patient.ID = newPatient.ID;

            var profile = new Data.Model.PatientProfile()
            {
                PatientID = patient.ID
            };

            await _profileRepository.SaveAsync(profile);

	        var newPatientAccount = new Data.Model.PatientAccount()
	        {
		        PatientID = newPatient.ID,
		        MobilePhone = patient.PatientAccount.MobilePhone,
                Email = patient.Email,
		        Password = patient.PatientAccount.Password,
		        IsActive = false,
		        UniqueKey = Guid.NewGuid(),
		        SmsActivationCode = new Random().Next(10000, 99999),
		        IsPhoneVerified = false,
                DateCreated = DateTime.UtcNow,
                IsEmailVerified = false
            };

	        result = await _accountRepository.SaveAsync(newPatientAccount);

	        if (result.MessageKey != Data.Crud.Success)
		        return new CrudResponse(Crud.Error);
            
	        return new CrudResponse(Crud.Success);
        }

        public async Task<Patient> GetProfile(int patientID)
        {
            var query = from c in _repository.Entity
                let city = (from city in _repository.Context.Cities where city.ID == c.CityID select city).FirstOrDefault() ?? new City()
                join k in _repository.Context.PatientProfiles on c.ID equals k.PatientID
                where c.ID == patientID
                select new { c, k, city };

            var result = await query.Select(c => new Patient
            {
                FirstName = c.c.FirstName,
                LastName = c.c.LastName,
                MiddleName = c.c.MiddleName,
                Email = c.c.Email,
                PreferredLanguage = c.c.PreferredLanguage,
                CityID = c.c.CityID,
                CityName = c.city.Name,
                CityNameUz = c.city.NameUz,
                CityNameEn = c.city.NameEn,
                PatientProfile = c.k.Map<Data.Model.PatientProfile, PatientProfile>()
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<CrudResponse> SaveProfile(Patient model)
        {
            var query = _repository.Entity
                .Include(c => c.PatientProfile)
                .FirstOrDefault(c => c.ID == model.ID);

            if (query == null)
                return new CrudResponse(Crud.ItemNotFoundError);

            query.FirstName = model.FirstName;
            query.LastName = model.LastName;
            query.MiddleName = model.MiddleName;
            query.Email = model.Email;
            query.PreferredLanguage = model.PreferredLanguage;
            query.CityID = model.CityID;
            
            await _repository.Context.SaveChangesAsync();

            query.PatientProfile.DateOfBirth = model.PatientProfile.DateOfBirth;
            query.PatientProfile.IsMale = model.PatientProfile.IsMale;

            _profileRepository.Context.Attach(query.PatientProfile);
            await _profileRepository.Context.SaveChangesAsync();

            return new CrudResponse(Crud.Success);
        }
    }
}