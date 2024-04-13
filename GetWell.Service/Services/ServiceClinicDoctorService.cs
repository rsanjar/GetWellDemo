using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
    public class ServiceClinicDoctorService : BaseService<ServiceClinicDoctor, Data.Model.ServiceClinicDoctor>, IServiceClinicDoctorService
    {
        #region ctor

        private readonly IRepository<Data.Model.ServiceClinicDoctor> _repository;

        public ServiceClinicDoctorService(IRepository<Data.Model.ServiceClinicDoctor> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
        
        public async Task<List<ServiceClinicDoctor>> GetAllByDoctor(int clinicId, int doctorId, int? serviceCategoryId = null)
        {
            var query = from c in _repository.Context.ServiceClinics
                let scd = (from k in _repository.Entity
                    join cd in _repository.Context.ClinicDoctors on k.ClinicDoctorID equals cd.ID
                    where k.ServiceClinicID == c.ID && cd.DoctorID == doctorId && cd.ClinicID == clinicId
                    select k).FirstOrDefault() ?? new Data.Model.ServiceClinicDoctor()
                join cd in _repository.Context.ClinicDoctors on c.ClinicID equals cd.ClinicID
                join service in _repository.Context.Services on c.ServiceID equals service.ID
                join cat in _repository.Context.ServiceCategories on service.ServiceCategoryID equals cat.ID
                orderby cat.Name, service.Name
                where c.ClinicID == clinicId && cd.ClinicID == clinicId && cd.DoctorID == doctorId && c.IsActive == true
                select new { c, scd, service, cd, cat };
            
            if (serviceCategoryId != null)
            {
                query = query.Where(c => c.service.ServiceCategoryID == serviceCategoryId);
            }

            var result = (await query.ToListAsync()).Select(c => new ServiceClinicDoctor
            {
                ID = c.scd?.ID ?? 0,
                AverageDuration = c.scd?.AverageDuration ?? c.c.AverageDuration,
                DateCreated = c.scd?.DateCreated ?? c.c.DateCreated,
                Price = c.scd?.Price ?? c.c.Price,
                IsActive = (c.scd?.IsActive ?? false),
                ClinicID = clinicId,
                ServiceID = c.c.ID,
                DateUpdated = c.scd?.DateUpdated,
                ClinicDoctorID = c.cd.ID,
                ServiceClinicID = c.c.ID,
                Description = c.scd?.Description,
                DescriptionEn = c.scd?.DescriptionEn,
                DescriptionUz = c.scd?.DescriptionUz,
                DoctorID = doctorId,
                Service = new DTO.Service
                {
                    ID = c.c.ID,
                    Name = c.service.Name,
                    Description = c.service.Description,
                    ServiceCategoryID = c.cat.ID,
                    ServiceCategoryName = c.cat.Name,
                    SortOrder = c.c.SortOrder,
                    IsActive = c.c.IsActive,
                    IconUrl = c.service.IconUrl
                }
            });

            return result.ToList();
        }
        
        public async Task<int> GetID(int serviceClinicID, int clinicDoctorID)
        {
            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.ServiceClinicID == serviceClinicID && c.ClinicDoctorID == clinicDoctorID);

            return query?.ID ?? 0;
        }

        public async Task<int> GetClinicID(int id)
        {
            var query = from c in _repository.Entity
                        join k in _repository.Context.ServiceClinics on c.ServiceClinicID equals k.ID
                        join f in _repository.Context.Clinics on k.ClinicID equals f.ID
                        where c.ID == id
                        select f;

            var result = await query.FirstOrDefaultAsync();

            return result?.ID ?? 0;
        }
    }
}