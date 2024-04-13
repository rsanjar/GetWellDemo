using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IRegionService : IBaseService<Region>
	{
		Task<List<Region>> GetAllAsync(int cityID);
	}
}