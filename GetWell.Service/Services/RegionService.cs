using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class RegionService : BaseService<Region, Data.Model.Region>, IRegionService
	{
        #region ctor

        private readonly IRepository<Data.Model.Region> _repository;

        public RegionService(IRepository<Data.Model.Region> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

		public async Task<List<Region>> GetAllAsync(int cityID)
        {
            var query = _repository.Entity
                .Where(c => c.CityID == cityID);

            var result = await ToListAsync(query);

			return result;
		}
	}
}