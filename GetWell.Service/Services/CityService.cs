using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class CityService : BaseService<City, Data.Model.City>, ICityService
	{
        #region ctor

        private readonly IRepository<Data.Model.City> _repository;

        public CityService(IRepository<Data.Model.City> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<PaginatedList<City>> GetAllAsync(int countryID, PaginatedList<City> pagination)
        {
            var result = await _repository.Entity
                .Where(c => c.CountryID == countryID)
                .GetPaginatedListAsync(pagination);
            
            return result;
        }

        public async Task<List<City>> GetAllAsync(int countryID)
        {
            var query = _repository.Entity
                .Where(c => c.CountryID == countryID)
                .OrderBy(c => c.Name);

            var result = await ToListAsync(query);

            return result;
        }

        public async Task<City> GetByRegionAsync(int regionId)
        {
            var query = from c in _repository.Entity
                join k in _repository.Context.Regions on c.ID equals k.CityID
                where k.ID == regionId
                select c;

            var result = await FirstOrDefaultAsync(query);

            return result;
        }
    }
}