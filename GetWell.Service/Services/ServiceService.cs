using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class ServiceService : BaseService<DTO.Service, Data.Model.Service>, IServiceService
	{
        #region ctor

        private readonly IRepository<Data.Model.Service> _repository;
		
        public ServiceService(IRepository<Data.Model.Service> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

		public async Task<List<DTO.Service>> GetAllAsync(int serviceCategoryID)
		{
			var query = _repository.Entity
				.Where(c => c.ServiceCategoryID == serviceCategoryID);

            var result = await ToListAsync(query);

			return result;
		}

		public async Task<List<DTO.Service>> GetAllAsync(int clinicID, int serviceCategoryID)
		{
			var query = from c in _repository.Entity 
					join s in _repository.Context.ServiceClinics on c.ID equals s.ServiceID
					where c.ServiceCategoryID == serviceCategoryID && s.ClinicID == clinicID && c.IsActive == true && s.IsActive == true
					select c;

            var result = await ToListAsync(query);

			return result;
		}

        public async Task<PaginatedList<DTO.Service>> GetAllAsync(ServiceSearch search)
        {
            var query = _repository.Entity
                .Join(_repository.Context.ServiceCategories, c => c.ServiceCategoryID, k => k.ID, (c, k) => new { c, k });
            
            if (search.ServiceCategoryID > 0)
                query = query.Where(c => c.c.ServiceCategoryID == search.ServiceCategoryID);

            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(c => EF.Functions.Like(c.c.Name, $"%{search.Name}%") || 
                                         EF.Functions.Like(c.c.NameUz, $"%{search.Name}%") ||
                                         EF.Functions.Like(c.c.NameEn, $"%{search.Name}%"));

            var result = await query.Select(c => new DTO.Service
            {
                ID = c.c.ID,
                Name = c.c.Name,
                NameUz = c.c.NameUz,
                NameEn = c.c.NameEn,
                IsActive = c.c.IsActive,
                Description = c.c.Description,
                ServiceCategoryID = c.c.ServiceCategoryID,
                ServiceCategoryName = c.k.Name,
                SortOrder = c.c.SortOrder,
                IconUrl = c.c.IconUrl
            }).GetPaginatedListAsync(search);


            return result;
        }

        public async Task<PaginatedList<DTO.Service>> AutoCompleteSearchAsync(string term, int pageSize = 5)
        {
            var query = _repository.Entity.Where(c => EF.Functions.Like(c.Name, $"%{term}%") || 
                                                      EF.Functions.Like(c.NameUz, $"%{term}%") ||
                                                      EF.Functions.Like(c.NameEn, $"%{term}%"));
               
            var result = await query.Select(c => new DTO.Service
            {
                ID = c.ID,
                Name = c.Name,
                NameUz = c.NameUz,
                NameEn = c.NameEn
            }).GetPaginatedListAsync(new PaginatedList<DTO.Service>(pageSize: pageSize));
            
            return result;
        }
	}
}