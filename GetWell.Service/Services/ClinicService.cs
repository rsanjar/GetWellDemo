using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class ClinicService : BaseService<Clinic, Data.Model.Clinic>, IClinicService
	{
		#region ctor

		private readonly IRepository<Data.Model.Clinic> _repository;

		public ClinicService(IRepository<Data.Model.Clinic> repository) : base(repository)
		{
			_repository = repository;
		}

		#endregion

		public async Task<PaginatedList<Clinic>> GetAllAsync(PaginatedList<Clinic> pagination, int cityID = 0, int patientID = 0)
		{
			var query = from clinic in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Average(r => r.PatientRating)
                let reviewCount = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Count()
				let favorite = patientID > 0 && (from fav in _repository.Context.PatientFavoriteClinics
					where fav.ClinicID == clinic.ID && fav.PatientID == patientID
					select fav).Any()
				join region in _repository.Context.Regions on clinic.RegionID equals region.ID
		        join account in _repository.Context.ClinicAccounts on clinic.ID equals account.ClinicID
		        join city in _repository.Context.Cities on region.CityID equals city.ID
				where account.IsActive == true && clinic.IsActive == true
		        select new { clinic, region, account, city, rating, favorite, reviewCount };

	        if (cityID > 0)
		        query = query.Where(c => c.city.ID == cityID);

            var queryResult = query.Select(c => new Clinic
            {
                ID = c.clinic.ID,
                Name = c.clinic.Name,
                IsPrivate = c.clinic.IsPrivate,
                IsPatientFavorite = c.favorite,
                Address = c.clinic.Address,
                LogoUrl = c.clinic.LogoUrl,
                IsFeatured = c.clinic.IsFeatured,
                Website = c.clinic.Website,
                CityName = c.city.Name,
                District = c.clinic.District,
                RegionName = c.region.Name,
                DateCreated = c.clinic.DateCreated,
                BusinessStartDate = c.clinic.BusinessStartDate,
                BusinessEndDate = c.clinic.BusinessEndDate,
                IsActive = c.clinic.IsActive,
                NameEn = c.clinic.NameEn,
                NameUz = c.clinic.NameUz,
                AddressEn = c.clinic.AddressEn,
                AddressUz = c.clinic.AddressUz,
                CityNameEn = c.city.NameEn,
                CityNameUz = c.city.NameUz,
                DistrictEn = c.clinic.DistrictEn,
                DistrictUz = c.clinic.DistrictUz,
                Rating = c.rating ?? 0,
                ReviewCount = c.reviewCount,
                CityID = c.city.ID,
                RegionID = c.region.ID,
                Description = c.clinic.Description,
                DescriptionEn = c.clinic.DescriptionEn,
                DescriptionUz = c.clinic.DescriptionUz,
                Latitude = c.clinic.Latitude,
                Longitude = c.clinic.Longitude,
                Street = c.clinic.Street,
                StreetEn = c.clinic.StreetEn,
                StreetUz = c.clinic.StreetUz,
                ThumbnailUrl = c.clinic.ThumbnailUrl,
                UniqueKey = c.clinic.UniqueKey,
                SortOrder = c.clinic.SortOrder
            });

            var result = await queryResult.GetPaginatedListAsync(pagination);

            return result;
        }
		
        public async Task<PaginatedList<Clinic>> GetAllAsync(int serviceCategoryID, PaginatedList<Clinic> pagination, int patientID = 0)
		{
			var query = (from c in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
					join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == c.ID
                    select ap).Average(r => r.PatientRating)
                let reviewCount = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == c.ID
                    select ap).Count()
                let favorite = patientID > 0 && (from fav in _repository.Context.PatientFavoriteClinics
                    where fav.ClinicID == c.ID && fav.PatientID == patientID
                    select fav).Any()
                join account in _repository.Context.ClinicAccounts on c.ID equals account.ClinicID
                join k in _repository.Context.ServiceClinics on c.ID equals k.ClinicID
				join s in _repository.Context.Services on k.ServiceID equals s.ID
                join scd in _repository.Context.ServiceClinicDoctors on k.ID equals scd.ServiceClinicID
                join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
				join a in _repository.Context.ServiceCategories on s.ServiceCategoryID equals a.ID
				join reg in _repository.Context.Regions on c.RegionID equals reg.ID
				join city in _repository.Context.Cities on reg.CityID equals city.ID
				where a.ID == serviceCategoryID && c.IsActive == true && k.IsActive == true && 
                      s.IsActive == true && a.IsActive == true && cd.IsActive == true && 
                      account.IsActive == true && scd.IsActive == true
				select new Clinic
				{
					ID = c.ID,
					Name = c.Name,
					NameEn = c.NameEn,
					NameUz = c.NameUz,
					Address = c.Address,
					AddressEn = c.AddressEn,
					AddressUz = c.AddressUz,
					District = c.District,
					DistrictEn = c.DistrictEn,
					DistrictUz = c.DistrictUz,
					IsFeatured = c.IsFeatured,
					IsPrivate = c.IsPrivate,
					Latitude = c.Latitude,
					Longitude = c.Longitude,
					ThumbnailUrl = c.ThumbnailUrl,
					Website = c.Website,
					Street = c.Street,
					StreetEn = c.StreetEn,
					StreetUz = c.StreetUz,
					RegionID = c.RegionID,
					Rating = rating ?? 0,
					ReviewCount = reviewCount,
					IsPatientFavorite = favorite,
					CityName = city.Name,
					CityNameEn = city.NameEn,
					CityNameUz = city.NameUz,
					DateCreated = c.DateCreated,
					DateUpdated = c.DateUpdated,
					BusinessStartDate = c.BusinessStartDate,
					BusinessEndDate = c.BusinessEndDate,
					IsActive = c.IsActive,
					SortOrder = c.SortOrder
                }).Distinct();

			var result = await query.GetPaginatedListAsync(pagination);

			return result;
		}

		public async Task<PaginatedList<Clinic>> SearchAsync(int serviceID, PaginatedList<Clinic> pagination, int patientID = 0)
		{
			var query = (from c in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == c.ID && sc.ServiceID == serviceID
                    select ap).Average(r => r.PatientRating)
                let reviewCount = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == c.ID && sc.ServiceID == serviceID
                    select ap).Count()
                let favorite = patientID > 0 && (from fav in _repository.Context.PatientFavoriteClinics
                    where fav.ClinicID == c.ID && fav.PatientID == patientID
                    select fav).Any()
                join account in _repository.Context.ClinicAccounts on c.ID equals account.ClinicID
                join k in _repository.Context.ServiceClinics on c.ID equals k.ClinicID
				join s in _repository.Context.Services on k.ServiceID equals s.ID
                join scd in _repository.Context.ServiceClinicDoctors on k.ID equals scd.ServiceClinicID
                join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                join a in _repository.Context.ServiceCategories on s.ServiceCategoryID equals a.ID
				join reg in _repository.Context.Regions on c.RegionID equals reg.ID
				join city in _repository.Context.Cities on reg.CityID equals city.ID
                         where s.ID == serviceID && c.IsActive == true && k.IsActive == true && 
                               s.IsActive == true && a.IsActive == true && cd.IsActive == true &&
                               scd.IsActive == true && account.IsActive == true
                         select new Clinic
				{
					ID = c.ID,
					Name = c.Name,
					NameEn = c.NameEn,
					NameUz = c.NameUz,
					Address = c.Address,
					AddressEn = c.AddressEn,
					AddressUz = c.AddressUz,
					District = c.District,
					DistrictEn = c.DistrictEn,
					DistrictUz = c.DistrictUz,
					IsFeatured = c.IsFeatured,
					IsPrivate = c.IsPrivate,
					Latitude = c.Latitude,
					Longitude = c.Longitude,
					ThumbnailUrl = c.ThumbnailUrl,
					Website = c.Website,
					Street = c.Street,
					StreetEn = c.StreetEn,
					StreetUz = c.StreetUz,
					RegionID = c.RegionID,
					Rating = rating ?? 0,
					ReviewCount = reviewCount,
					IsPatientFavorite = favorite,
					CityName = city.Name,
					CityNameEn = city.NameEn,
					CityNameUz = city.NameUz,
					DateCreated = c.DateCreated,
					DateUpdated = c.DateUpdated,
					BusinessStartDate = c.BusinessStartDate,
					BusinessEndDate = c.BusinessEndDate,
					IsActive = c.IsActive,
					SortOrder = c.SortOrder,
					ServiceClinic = new ServiceClinic()
                    {
						ID = k.ID,
						ServiceID = s.ID,
                        Name = s.Name,
						NameEn = s.NameEn,
						NameUz = s.NameUz,
						AverageDuration = k.AverageDuration,
						Price = k.Price,
						IsActive = k.IsActive
                    }
                }).Distinct();

			var result = await query.GetPaginatedListAsync(pagination);

			return result;
		}

        public async Task<PaginatedList<Clinic>> GetAllAsync(ClinicSearch search)
        {
	        var query = from clinic in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Average(r => r.PatientRating)
		        join region in _repository.Context.Regions on clinic.RegionID equals region.ID
		        join account in _repository.Context.ClinicAccounts on clinic.ID equals account.ClinicID
		        join city in _repository.Context.Cities on region.CityID equals city.ID
                select new { clinic, region, account, city, rating };

            if (search.IsActive.HasValue)
                query = query.Where(c => c.clinic.IsActive == search.IsActive || c.account.IsActive == search.IsActive);

            if (search.IsFeatured.HasValue)
                query = query.Where(c => c.clinic.IsFeatured == search.IsFeatured);

            if (search.CityID > 0)
                query = query.Where(c => c.city.ID == search.CityID);

			if(search.RegionID > 0)
                query = query.Where(c => c.region.ID == search.RegionID);

            if (!string.IsNullOrWhiteSpace(search.ClinicName))
                query = query.Where(c => EF.Functions.Like(c.clinic.Name, $"%{search.ClinicName}%"));

            if (search.RegistrationDateStart.HasValue)
                query = query.Where(c => c.account.DateCreated >= search.RegistrationDateStart.Value);

            if (search.RegistrationDateEnd.HasValue)
                query = query.Where(c => c.account.DateCreated <= search.RegistrationDateEnd.Value);

            var result = await query.Select(c => new Clinic
            {
				ID = c.clinic.ID,
				Name = c.clinic.Name,
				IsPrivate = c.clinic.IsPrivate,
				Address = c.clinic.Address,
				LogoUrl = c.clinic.LogoUrl,
				IsFeatured = c.clinic.IsFeatured,
				Website = c.clinic.Website,
				CityName = c.city.Name,
				District = c.clinic.District,
				RegionName = c.region.Name,
				DateCreated = c.clinic.DateCreated,
                BusinessStartDate = c.clinic.BusinessStartDate,
                BusinessEndDate = c.clinic.BusinessEndDate,
                IsActive = c.clinic.IsActive,
				NameEn = c.clinic.NameEn,
				NameUz = c.clinic.NameUz,
				AddressEn = c.clinic.AddressEn,
				AddressUz = c.clinic.AddressUz,
				CityNameEn = c.city.NameEn,
				CityNameUz = c.city.NameUz,
				DistrictEn = c.clinic.DistrictEn,
				DistrictUz = c.clinic.DistrictUz,
				Rating = c.rating ?? 0,
				CityID = c.city.ID,
				RegionID = c.region.ID,
				Description = c.clinic.Description,
				DescriptionEn = c.clinic.DescriptionEn,
				DescriptionUz = c.clinic.DescriptionUz,
				Latitude = c.clinic.Latitude,
				Longitude = c.clinic.Longitude,
				Street = c.clinic.Street,
				StreetEn = c.clinic.StreetEn,
				StreetUz = c.clinic.StreetUz,
				ThumbnailUrl = c.clinic.ThumbnailUrl,
				UniqueKey = c.clinic.UniqueKey,
				SortOrder = c.clinic.SortOrder
            }).GetPaginatedListAsync(search);


            return result;
        }

		public async Task<PaginatedList<Clinic>> GetAllByCategoryAsync(int categoryID, PaginatedList<Clinic> pagination, int cityID = 0, int patientID = 0)
		{
			var query = from clinic in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Average(r => r.PatientRating)
                let reviewCount = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Count()
				let favorite = patientID > 0 && (from fav in _repository.Context.PatientFavoriteClinics
					where fav.ClinicID == clinic.ID && fav.PatientID == patientID
					select fav).Any()
				join region in _repository.Context.Regions on clinic.RegionID equals region.ID
		        join account in _repository.Context.ClinicAccounts on clinic.ID equals account.ClinicID
		        join city in _repository.Context.Cities on region.CityID equals city.ID
				join serviceClinic in _repository.Context.ServiceClinics on clinic.ID equals serviceClinic.ClinicID
				join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
                join serviceClinicDoctor in _repository.Context.ServiceClinicDoctors on serviceClinic.ID equals serviceClinicDoctor.ServiceClinicID
                join clinicDoctor in _repository.Context.ClinicDoctors on serviceClinicDoctor.ClinicDoctorID equals clinicDoctor.ID
				join serviceCategory in _repository.Context.ServiceCategories on service.ServiceCategoryID equals serviceCategory.ID
		        where serviceCategory.CategoryID == categoryID &&
                      account.IsActive == true && clinic.IsActive == true && serviceClinic.IsActive == true &&
                      service.IsActive == true && serviceClinicDoctor.IsActive == true && clinicDoctor.IsActive == true
                //&& city.ID == cityID
                        select new { clinic, region, account, city, rating, favorite, reviewCount };

	        if (cityID > 0)
		        query = query.Where(c => c.city.ID == cityID);

            var result = await query.Select(c => new Clinic
            {
				ID = c.clinic.ID,
				Name = c.clinic.Name,
				IsPrivate = c.clinic.IsPrivate,
				IsPatientFavorite = c.favorite,
				Address = c.clinic.Address,
				LogoUrl = c.clinic.LogoUrl,
				IsFeatured = c.clinic.IsFeatured,
				Website = c.clinic.Website,
				CityName = c.city.Name,
				District = c.clinic.District,
				RegionName = c.region.Name,
				DateCreated = c.clinic.DateCreated,
                BusinessStartDate = c.clinic.BusinessStartDate,
                BusinessEndDate = c.clinic.BusinessEndDate,
                IsActive = c.clinic.IsActive,
				NameEn = c.clinic.NameEn,
				NameUz = c.clinic.NameUz,
				AddressEn = c.clinic.AddressEn,
				AddressUz = c.clinic.AddressUz,
				CityNameEn = c.city.NameEn,
				CityNameUz = c.city.NameUz,
				DistrictEn = c.clinic.DistrictEn,
				DistrictUz = c.clinic.DistrictUz,
				Rating = c.rating ?? 0,
				ReviewCount = c.reviewCount,
				CityID = c.city.ID,
				RegionID = c.region.ID,
				Description = c.clinic.Description,
				DescriptionEn = c.clinic.DescriptionEn,
				DescriptionUz = c.clinic.DescriptionUz,
				Latitude = c.clinic.Latitude,
				Longitude = c.clinic.Longitude,
				Street = c.clinic.Street,
				StreetEn = c.clinic.StreetEn,
				StreetUz = c.clinic.StreetUz,
				ThumbnailUrl = c.clinic.ThumbnailUrl,
				UniqueKey = c.clinic.UniqueKey,
				SortOrder = c.clinic.SortOrder
            }).Distinct().GetPaginatedListAsync(pagination);


            return result;
        }

        public async Task<List<Clinic>> GetAllByCityAsync(int cityID)
        {
            var query = _repository.Context.Clinics
                .Join(_repository.Context.Regions, c => c.RegionID, r => r.ID, (c, r) => new { c, r })
                .Join(_repository.Context.ClinicAccounts, @t => @t.c.ID, a => a.ClinicID, (@t, a) => new { @t, a })
                .Join(_repository.Context.Cities, @t => @t.@t.r.CityID, k => k.ID, (@t, k) => new { @t, k })
                .Where(@t => @t.t.a.IsActive == true);

            if (cityID > 0)
                query = query.Where(@t => @t.t.t.r.CityID == cityID).OrderBy(k => k.t.t.c.Name);
			
            var result = await query.Select(c => new Clinic
            {
                ID = c.t.t.c.ID,
                Name = c.t.t.c.Name,
				NameEn = c.t.t.c.NameEn,
				NameUz = c.t.t.c.NameUz
            }).ToListAsync();
			
            return result;
        }
		
        public async Task<List<Clinic>> GetAllAsync(int regionID)
        {
	        if (regionID <= 0)
		        return new List<Clinic>();

	        var query = _repository.Context.Clinics
		        .Join(_repository.Context.Regions, c => c.RegionID, r => r.ID, (c, r) => new {c, r})
		        .Join(_repository.Context.ClinicAccounts, @t => @t.c.ID, a => a.ClinicID, (@t, a) => new {@t, a})
		        .Join(_repository.Context.Cities, @t => @t.@t.r.CityID, k => k.ID, (@t, k) => new {@t, k})
		        .Where(@t => @t.t.a.IsActive == true && @t.t.t.r.ID == regionID)
		        .OrderBy(k => k.t.t.c.Name);

	        var result = await query.Select(c => new Clinic
	        {
		        ID = c.t.t.c.ID,
		        Name = c.t.t.c.Name,
		        IsPrivate = c.t.t.c.IsPrivate,
		        Address = c.t.t.c.Address,
		        LogoUrl = c.t.t.c.LogoUrl,
		        IsFeatured = c.t.t.c.IsFeatured,
		        Website = c.t.t.c.Website,
		        CityName = c.k.Name,
		        District = c.t.t.c.District,
		        RegionName = c.t.t.r.Name,
		        DateCreated = c.t.a.DateCreated,
		        BusinessStartDate = c.t.t.c.BusinessStartDate,
		        BusinessEndDate = c.t.t.c.BusinessEndDate,
		        IsActive = c.t.t.c.IsActive,
				SortOrder = c.t.t.c.SortOrder

	        }).OrderByDescending(c => c.IsFeatured).ToListAsync();
			
	        return result;
        }

        public override async Task<Clinic> GetAsync(int id)
        {
            var query = from clinic in _repository.Context.Clinics
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
                    select ap).Average(r => r.PatientRating)
                let reviewCount = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    join cd in _repository.Context.ClinicDoctors on scd.ClinicDoctorID equals cd.ID
                    where a.IsArchived && ap.PatientRating != null && sc.ClinicID == cd.ClinicID && sc.ClinicID == clinic.ID
			select ap).Count()
                join region in _repository.Context.Regions on clinic.RegionID equals region.ID
		        join account in _repository.Context.ClinicAccounts on clinic.ID equals account.ClinicID
		        join city in _repository.Context.Cities on region.CityID equals city.ID
				where clinic.ID == id
		        select new { clinic, region, account, city, rating, reviewCount };
			
            var result = await query.Select(c => new Clinic
            {
				ID = c.clinic.ID,
				Name = c.clinic.Name,
				IsPrivate = c.clinic.IsPrivate,
				Address = c.clinic.Address,
				LogoUrl = c.clinic.LogoUrl,
				IsFeatured = c.clinic.IsFeatured,
				Website = c.clinic.Website,
				CityName = c.city.Name,
				District = c.clinic.District,
				RegionName = c.region.Name,
				DateCreated = c.clinic.DateCreated,
                BusinessStartDate = c.clinic.BusinessStartDate,
                BusinessEndDate = c.clinic.BusinessEndDate,
                IsActive = c.clinic.IsActive,
				NameEn = c.clinic.NameEn,
				NameUz = c.clinic.NameUz,
				AddressEn = c.clinic.AddressEn,
				AddressUz = c.clinic.AddressUz,
				CityNameEn = c.city.NameEn,
				CityNameUz = c.city.NameUz,
				DistrictEn = c.clinic.DistrictEn,
				DistrictUz = c.clinic.DistrictUz,
				Rating = c.rating ?? 0,
				ReviewCount = c.reviewCount,
				CityID = c.city.ID,
				RegionID = c.region.ID,
				Description = c.clinic.Description,
				DescriptionEn = c.clinic.DescriptionEn,
				DescriptionUz = c.clinic.DescriptionUz,
				Latitude = c.clinic.Latitude,
				Longitude = c.clinic.Longitude,
				Street = c.clinic.Street,
				StreetEn = c.clinic.StreetEn,
				StreetUz = c.clinic.StreetUz,
				ThumbnailUrl = c.clinic.ThumbnailUrl,
				UniqueKey = c.clinic.UniqueKey,
				SortOrder = c.clinic.SortOrder
            }).FirstOrDefaultAsync();


            return result;
        }

        public async Task<Clinic> GetAsync(Guid uniqueKey)
        {
            var result = await _repository.Entity
                .Where(c => c.UniqueKey == uniqueKey)
                .FirstOrDefaultAsync<Data.Model.Clinic, Clinic>();

            return result;
        }

        public async Task<string> GetQrImageBase64Async(int clinicID)
        {
            var result = await _repository.Entity
                .Where(c => c.ID == clinicID)
                .Select(c => c.QrImageCode)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<CrudResponse> SaveQrImageBase64Async(int clinicID, string base64Image)
        {
            var item = await _repository.Entity.FirstOrDefaultAsync(c => c.ID == clinicID);

            if (item == null)
                return new CrudResponse(Crud.ItemNotFoundError);

            item.QrImageCode = base64Image;
			await _repository.Context.SaveChangesAsync();

            return new CrudResponse(Crud.Success);
        }

        public async Task<Guid> GetUniqueKey(int clinicID)
        {
            var result = await _repository.Entity
                .Where(c => c.ID == clinicID)
                .Select(c => c.UniqueKey)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<PaginatedList<Clinic>> AutoCompleteSearchAsync(string term, int pageSize = 5)
        {
            var query = _repository.Entity.Where(c => EF.Functions.Like(c.Name, $"%{term}%") || 
                                                      EF.Functions.Like(c.NameUz, $"%{term}%") ||
                                                      EF.Functions.Like(c.NameEn, $"%{term}%"));
		
            var result = await query.Where(c => c.IsActive == true).Select(c => new Clinic
            {
                ID = c.ID,
                Name = c.Name,
                NameUz = c.NameUz,
                NameEn = c.NameEn
            }).GetPaginatedListAsync(new PaginatedList<Clinic>(pageSize: pageSize));
            
            return result;
        }
    }
}