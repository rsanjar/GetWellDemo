using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class CategoryService : BaseService<Category, Data.Model.Category>, ICategoryService
	{
        #region ctor

        private readonly IRepository<Data.Model.Category> _repository;

        public CategoryService(IRepository<Data.Model.Category> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion
        
		public async Task<List<Category>> GetAllAsync(bool getOnlyActive = true)
		{
			var query = _repository.Entity
                .OrderBy(c => c.SortOrder)
                .Select(c => c);
            
            if (getOnlyActive)
                query = query.Where(c => c.IsActive == true);

            var result = await ToListAsync(query);

			return result;
		}
	}
}