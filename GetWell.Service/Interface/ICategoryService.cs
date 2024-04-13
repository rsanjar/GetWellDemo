using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface ICategoryService : IBaseService<Category>
	{
		Task<List<Category>> GetAllAsync(bool getOnlyActive = true);
	}
}