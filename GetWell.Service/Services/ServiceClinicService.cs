using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;

namespace GetWell.Service.Services
{
    public class ServiceClinicService : BaseService<ServiceClinic, Data.Model.ServiceClinic>, IServiceClinicService
    {
        #region ctor

        private readonly IRepository<Data.Model.ServiceClinic> _repository;

        public ServiceClinicService(IRepository<Data.Model.ServiceClinic> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<ServiceClinic>> GetAllAsync(int clinicId)
        {
            var query = from c in _repository.Entity
                join k in _repository.Context.Services on c.ServiceID equals k.ID
                orderby k.ServiceCategory.Name, k.Name
                where c.ClinicID == clinicId && c.IsActive == true
                orderby k.Name
                select new ServiceClinic
                {
                    ID = c.ID,
                    AverageDuration = c.AverageDuration,
                    DateCreated = c.DateCreated,
                    Price = c.Price,
                    ServiceID = c.ServiceID,
                    SortOrder = c.SortOrder,
                    IsActive = c.IsActive,
                    ClinicID = c.ClinicID,
                    DateUpdated = c.DateUpdated,
                    Name = k.Name,
                    NameEn = k.NameEn,
                    NameUz = k.NameUz,
                    Service = new DTO.Service
                    {
                        ID = k.ID,
                        Name = k.Name,
                        NameEn = k.NameEn,
                        NameUz = k.NameUz,
                        Description = k.Description,
                        DescriptionEn = k.DescriptionEn,
                        DescriptionUz = k.DescriptionUz,
                        ServiceCategoryID = k.ServiceCategoryID,
                        ServiceCategoryName = k.ServiceCategory.Name,
                        SortOrder = k.SortOrder,
                        IsActive = k.IsActive,
                        IconUrl = k.IconUrl
                    }
                };

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<List<ServiceClinic>> GetAllByClinicAsync(int clinicId, int? serviceCategoryId = null)
        {
            var query = from c in _repository.Context.Services 
                join k in (from f in _repository.Context.ServiceClinics where f.ClinicID == clinicId select f) on c.ID equals k.ServiceID into g
                from k in g.DefaultIfEmpty()
                orderby c.ServiceCategory.Name, c.Name
                select new
                {
                    c, 
                    s = k ?? new Data.Model.ServiceClinic()
                };

            if (serviceCategoryId.GetValueOrDefault(0) > 0)
                query = query.Where(c => c.c.ServiceCategoryID == serviceCategoryId);
            
            var result = query.Select(c => new ServiceClinic
            {
                ID = c.s.ID,
                AverageDuration = c.s.AverageDuration,
                DateCreated = c.s.DateCreated,
                Price = c.s.Price,
                ServiceID = c.c.ID,
                SortOrder = c.s.SortOrder,
                IsActive = c.s.IsActive,
                ClinicID = clinicId,
                DateUpdated = c.s.DateUpdated,
                Name = c.c.Name,
                NameEn = c.c.NameEn,
                NameUz = c.c.NameUz,
                Service = new DTO.Service
                {
                    ID = c.c.ID,
                    Name = c.c.Name,
                    Description = c.c.Description,
                    ServiceCategoryID = c.c.ServiceCategoryID,
                    ServiceCategoryName = c.c.ServiceCategory.Name,
                    SortOrder = c.c.SortOrder,
                    IsActive = c.c.IsActive,
                    IconUrl = c.c.IconUrl
                }
            });

            return await result.ToListAsync();
        }

        public async Task<List<ServiceClinic>> GetAllByServiceCategoryAsync(int clinicId, int serviceCategoryId, bool getOnlyActive = false)
        {
	        var query = from c in _repository.Entity
		        join s in _repository.Context.Services on c.ServiceID equals s.ID
		        join sc in _repository.Context.ServiceCategories on s.ServiceCategoryID equals sc.ID
                where c.ClinicID == clinicId && sc.ID == serviceCategoryId
		        select c;

            if (getOnlyActive)
                query = query.Where(c => c.IsActive == true);

	        var result = await query.ProjectTo<ServiceClinic>(new MapperConfiguration(c =>
	        {
		        c.CreateMap<Data.Model.ServiceClinic, ServiceClinic>();
		        c.CreateMap<Data.Model.Service, DTO.Service>();
	        })).ToListAsync();

	        return result;
        }

        public async Task<PaginatedList<ServiceClinic>> SearchAsync(ServiceClinicSearch search)
        {
	        var query = from serviceClinic in _repository.Entity
                let rating = (from a in _repository.Context.Appointments
                    join ap in _repository.Context.AppointmentProfiles on a.ID equals ap.AppointmentID
                    join scd in _repository.Context.ServiceClinicDoctors on a.ServiceClinicDoctorID equals scd.ID
                    join sc in _repository.Context.ServiceClinics on scd.ServiceClinicID equals sc.ID
                    where a.IsArchived && scd.ServiceClinicID == serviceClinic.ID && serviceClinic.ClinicID == sc.ClinicID && ap.PatientRating != null
                    select ap).Average(r => r.PatientRating)
		        join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
		        join clinic in _repository.Context.Clinics on serviceClinic.ClinicID equals clinic.ID
                join region in _repository.Context.Regions on clinic.RegionID equals region.ID
                join city in _repository.Context.Cities on region.CityID equals city.ID
                where service.ID == search.ServiceID && city.ID == search.CityID &&
                      serviceClinic.IsActive == true &&
                      clinic.IsActive == true && service.IsActive == true
                      
                select new ServiceClinic
		        {
			        ID = serviceClinic.ID,
			        AverageDuration = serviceClinic.AverageDuration,
			        DateCreated = serviceClinic.DateCreated,
			        Price = serviceClinic.Price,
			        ServiceID = serviceClinic.ServiceID,
			        SortOrder = serviceClinic.SortOrder,
			        IsActive = serviceClinic.IsActive,
			        ClinicID = serviceClinic.ClinicID,
                    DateUpdated = serviceClinic.DateUpdated,
                    Name = service.Name,
                    NameEn = service.NameEn,
                    NameUz = service.NameUz,
			        Service = new DTO.Service
			        {
				        ID = service.ID,
				        Name = service.Name,
				        NameEn = service.NameEn,
				        NameUz = service.NameUz,
				        Description = service.Description,
				        DescriptionEn = service.DescriptionEn,
				        DescriptionUz = service.DescriptionUz,
				        ServiceCategoryID = service.ServiceCategoryID,
				        ServiceCategoryName = service.ServiceCategory.Name,
				        SortOrder = service.SortOrder,
				        IsActive = service.IsActive,
				        IconUrl = service.IconUrl
			        },
                    Clinic = new Clinic
                    {
	                    ID = clinic.ID,
	                    IsPrivate = clinic.IsPrivate,
	                    LogoUrl = clinic.LogoUrl,
	                    IsFeatured = clinic.IsFeatured,
	                    Website = clinic.Website,
	                    District = clinic.District,
	                    RegionName = region.Name,
	                    DateCreated = clinic.DateCreated,
	                    BusinessStartDate = clinic.BusinessStartDate,
	                    BusinessEndDate = clinic.BusinessEndDate,
	                    IsActive = clinic.IsActive,
	                    Name = clinic.Name,
	                    NameEn = clinic.NameEn,
	                    NameUz = clinic.NameUz,
	                    Address = clinic.Address,
	                    AddressEn = clinic.AddressEn,
	                    AddressUz = clinic.AddressUz,
	                    CityName = city.Name,
	                    CityNameEn = city.NameEn,
	                    CityNameUz = city.NameUz,
	                    DistrictEn = clinic.DistrictEn,
	                    DistrictUz = clinic.DistrictUz,
	                    Rating = rating ?? 0,
	                    CityID = city.ID,
	                    RegionID = region.ID,
	                    Description = clinic.Description,
	                    DescriptionEn = clinic.DescriptionEn,
	                    DescriptionUz = clinic.DescriptionUz,
	                    Latitude = clinic.Latitude,
	                    Longitude = clinic.Longitude,
	                    Street = clinic.Street,
	                    StreetEn = clinic.StreetEn,
	                    StreetUz = clinic.StreetUz,
	                    ThumbnailUrl = clinic.ThumbnailUrl,
	                    UniqueKey = clinic.UniqueKey
                    }
		        };
            
            var result = await query.GetPaginatedListAsync(search);

	        return result;
        }

        public async Task<int> GetIDAsync(int serviceClinicDoctorID)
        {
	        var query = from c in _repository.Entity
		        join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ServiceClinicID
		        where k.ID == serviceClinicDoctorID
			        select c;

	        var result = await FirstOrDefaultAsync(query);

	        return result.ID;
        }

        public async Task<ServiceClinic> GetByClinicDoctorAsync(int serviceClinicDoctorID)
        {
	        var query = from c in _repository.Entity
		        join k in _repository.Context.ServiceClinicDoctors on c.ID equals k.ServiceClinicID
		        where k.ID == serviceClinicDoctorID
		        select c;

	        var result = await FirstOrDefaultAsync(query);

	        return result;
        }
    }
}