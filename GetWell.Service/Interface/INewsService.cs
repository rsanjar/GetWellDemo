using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface INewsService : IBaseService<News>
    {
        Task<List<News>> GetAllAsync(NewsSearch search);
    }
}