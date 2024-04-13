using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class DoctorService : BaseService<Doctor, Data.Model.Doctor>, IDoctorService
	{
        #region ctor

        private readonly IRepository<Data.Model.Doctor> _repository;
		
        public DoctorService(IRepository<Data.Model.Doctor> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

		public async Task<List<Doctor>> GetAllAsync(int clinicID)
		{
			var query = from c in _repository.Context.Doctors 
				join k in _repository.Context.ClinicDoctors on c.ID equals k.DoctorID
				where k.ClinicID == clinicID
				select c;
            
            var result = await query.ProjectTo<Doctor>(new MapperConfiguration(c =>
            {
                c.CreateMap<Data.Model.Doctor, Doctor>();
                c.CreateMap<Data.Model.DoctorProfile, DoctorProfile>();
                c.CreateMap<Data.Model.DoctorPhone, DoctorPhone>();
            })).ToListAsync();
            
			return result;
		}

        public async Task<List<Doctor>> GetAllAsync(DoctorSearch search)
        {
            var query = from c in _repository.Context.Doctors
                from k in _repository.Context.ClinicDoctors.Where(k => k.DoctorID == c.ID).DefaultIfEmpty()
                select new { c, k };

            if (search.ClinicID > 0)
                query = query.Where(c => c.k.ClinicID == search.ClinicID);
            else
            {
                if (search.CityID > 0)
                    query = query.Where(c => c.k.Clinic.Region.CityID == search.CityID);

                if (search.RegionID > 0)
                    query = query.Where(c => c.k.Clinic.RegionID == search.RegionID);
            }

            if (!string.IsNullOrWhiteSpace(search.FirstName))
                query = query.Where(c => EF.Functions.Like(c.k.Doctor.FirstName, $"%{search.FirstName}%"));

            if (!string.IsNullOrWhiteSpace(search.LastName))
                query = query.Where(c => EF.Functions.Like(c.k.Doctor.LastName, $"%{search.LastName}%"));

            if (!string.IsNullOrWhiteSpace(search.MiddleName))
                query = query.Where(c => EF.Functions.Like(c.k.Doctor.MiddleName, $"%{search.MiddleName}%"));

            if (search.DateOfBirthFrom.HasValue)
                query = query.Where(c => c.k.Doctor.DateOfBirth >= search.DateOfBirthFrom.Value);

            if (search.DateOfBirthTo.HasValue)
                query = query.Where(c => c.k.Doctor.DateOfBirth >= search.DateOfBirthTo.Value);

            var result = await query.Select(c => new Doctor
            {
                ID = c.c.ID,
                IsActive = c.c.IsActive,
                CareerStartDate = c.c.CareerStartDate,
                ClinicID = c.k.ClinicID,
                ClinicName = c.k.Clinic.Name,
                DateCreated = c.c.DateCreated,
                DateOfBirth = c.c.DateOfBirth,
                FirstName = c.c.FirstName,
                LastName = c.c.LastName,
                MiddleName = c.c.MiddleName,
                IsRetired = c.c.IsRetired,
                IsFamilyDoctor = c.c.IsFamilyDoctor

            }).GetPaginatedListAsync(search);

            return result;
        }

        public async Task<PaginatedList<Doctor>> AutoCompleteSearchAsync(string term, int pageSize = 5)
        {
            var query = _repository.Entity.Where(c => EF.Functions.Like(c.FirstName, $"%{term}%") || 
                                                      EF.Functions.Like(c.FirstNameUz, $"%{term}%") ||
                                                      EF.Functions.Like(c.FirstNameEn, $"%{term}%") ||
                                                      EF.Functions.Like(c.LastName, $"%{term}%") || 
                                                      EF.Functions.Like(c.LastNameUz, $"%{term}%") ||
                                                      EF.Functions.Like(c.LastNameEn, $"%{term}%") ||
                                                      EF.Functions.Like(c.MiddleName, $"%{term}%") || 
                                                      EF.Functions.Like(c.MiddleNameUz, $"%{term}%") ||
                                                      EF.Functions.Like(c.MiddleNameEn, $"%{term}%"));
               
            var result = await query.Select(c => new Doctor
            {
                ID = c.ID,
                FirstName = c.FirstName,
                FirstNameUz = c.FirstNameUz,
                FirstNameEn = c.FirstNameEn,
                LastName = c.LastName,
                LastNameUz = c.LastNameUz,
                LastNameEn = c.LastNameEn,
                MiddleName = c.MiddleName,
                MiddleNameUz = c.MiddleNameUz,
                MiddleNameEn = c.MiddleNameEn
            }).GetPaginatedListAsync(new PaginatedList<Doctor>(pageSize: pageSize));
            
            return result;
        }
	}
}