using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface ITitleService : IBaseService<Title>
    {
        Task<List<Title>> GetAllAsync();
    }
}