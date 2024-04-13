using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class TitleService : BaseService<Title, Data.Model.Title>, ITitleService
	{
        #region MyRegion

        private readonly IRepository<Data.Model.Title> _repository;

        public TitleService(IRepository<Data.Model.Title> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<Title>> GetAllAsync()
        {
            var query = _repository
                .Entity
                .OrderBy(c => c.Name)
                .Select(c => c);

            var result = await ToListAsync(query);

            return result;
        }
	}
}