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
    public class ServiceCategoryService : BaseService<ServiceCategory, Data.Model.ServiceCategory>, IServiceCategoryService
	{
        #region ctor

        private readonly IRepository<Data.Model.ServiceCategory> _repository;

        public ServiceCategoryService(IRepository<Data.Model.ServiceCategory> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
		
        public async Task<List<ServiceCategory>> GetAllAsync()
        {
            var query = from c in _repository.Entity
                join k in _repository.Context.Categories on c.CategoryID equals k.ID
                where k.IsActive == true && c.IsActive == true
                select c;

            var result = await ToListAsync(query);

            return result;
        }

		public async Task<List<ServiceCategory>> GetAllAsync(int categoryID)
		{
			var query = from c in _repository.Entity
				join k in _repository.Context.Categories on c.CategoryID equals k.ID
				where k.ID == categoryID && k.IsActive == true
				select c;

            var result = await ToListAsync(query);

			return result;
		}

		public async Task<List<ServiceCategory>> GetAllAsync(int clinicID, bool includeServices)
		{
			var query = (from c in _repository.Entity
				join s in _repository.Context.Services on c.ID equals s.ServiceCategoryID
				join a in _repository.Context.ServiceClinics on s.ID equals a.ServiceID
				where a.ClinicID == clinicID && s.IsActive == true && c.IsActive == true
				select c);

            List<ServiceCategory> result;

            if (includeServices)
                result = await query.ProjectTo<ServiceCategory>(new MapperConfiguration(c =>
                {
                    c.CreateMap<Data.Model.ServiceCategory, ServiceCategory>();
                    c.CreateMap<Data.Model.Service, DTO.Service>();
                    c.CreateMap<Data.Model.Title, Title>();
                })).Distinct().ToListAsync();
            else
            {
                result = await ToListAsync(query.Distinct());
            }
			
			return result;
		}

        public async Task<List<ServiceCategory>> GetAllActiveAsync(int clinicID, bool includeServices)
        {
            var query = (from c in _repository.Entity
                join s in _repository.Context.Services on c.ID equals s.ServiceCategoryID
                join a in _repository.Context.ServiceClinics on s.ID equals a.ServiceID
                where a.ClinicID == clinicID && s.IsActive == true && c.IsActive == true && a.IsActive == true
                select c);

            List<ServiceCategory> result;
            
            if (includeServices)
                result = await query.ProjectTo<ServiceCategory>(new MapperConfiguration(c =>
                {
                    c.CreateMap<Data.Model.ServiceCategory, ServiceCategory>();
                    c.CreateMap<Data.Model.Service, DTO.Service>();
                    c.CreateMap<Data.Model.Title, Title>();
                })).Distinct().ToListAsync();
            else
            {
                result = await ToListAsync(query.Distinct());
            }
			
            return result;
        }

		public async Task<int> GetIDAsync(int serviceClinicDoctorID)
		{
			var query = from c in _repository.Entity
					join s in _repository.Context.Services on c.ID equals s.ServiceCategoryID
					join sc in _repository.Context.ServiceClinics on s.ID equals sc.ServiceID
					join scd in _repository.Context.ServiceClinicDoctors on sc.ID equals scd.ServiceClinicID
					where scd.ID == serviceClinicDoctorID
					select c;

			var result = await FirstOrDefaultAsync(query);

			return result.ID;
		}
        
        public async Task<PaginatedList<ServiceCategory>> GetAllAsync(ServiceCategorySearch search)
        {
            var query = _repository.Entity
                .Join(_repository.Context.Categories, c => c.CategoryID, k => k.ID, (c, k) => new {c, k})
                .Join(_repository.Context.Titles, @t1 => @t1.c.TitleID, t => t.ID, (@t1, t) => new {@t1, t});
                //.Where(@t1 => @t1.@t1.c.IsActive == search.IsActive);
                
            
            if (search.CategoryID > 0)
                query = query.Where(c => c.t1.c.CategoryID == search.CategoryID);
            
            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(c => EF.Functions.Like(c.t1.c.Name, $"%{search.Name}%"));
            
            var result = await query.Select(c => new ServiceCategory
            {
                ID = c.t1.c.ID,
                Name = c.t1.c.Name,
                IsActive = c.t1.c.IsActive,
                Description = c.t1.c.Description,
                CategoryID = c.t1.c.CategoryID,
                HexColor = c.t1.c.HexColor,
                IconUrl = c.t1.c.IconUrl,
                SortOrder = c.t1.c.SortOrder,
                TitleID = c.t1.c.TitleID,
                CategoryName = c.t1.k.Name,
                TitleName = c.t.Name
            }).GetPaginatedListAsync(search);


            return result;
        }
	}
}