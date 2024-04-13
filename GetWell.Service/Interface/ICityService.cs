using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface ICityService : IBaseService<City>
    {
        Task<PaginatedList<City>> GetAllAsync(int countryID, PaginatedList<City> pagination);

        Task<List<City>> GetAllAsync(int countryID);

        Task<City> GetByRegionAsync(int regionId);
    }
}