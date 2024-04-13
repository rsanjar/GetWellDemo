using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;

namespace GetWell.Service.Interface
{
	public interface IServiceService : IBaseService<DTO.Service>
	{
		Task<List<DTO.Service>> GetAllAsync(int serviceCategoryID);

		Task<List<DTO.Service>> GetAllAsync(int clinicID, int serviceCategoryID);

        Task<PaginatedList<DTO.Service>> GetAllAsync(ServiceSearch search);

        Task<PaginatedList<DTO.Service>> AutoCompleteSearchAsync(string term, int pageSize = 5);
    }
}