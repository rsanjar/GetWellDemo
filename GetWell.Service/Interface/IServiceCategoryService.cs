using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IServiceCategoryService : IBaseService<ServiceCategory>
    {
        Task<List<ServiceCategory>> GetAllAsync();

		Task<List<ServiceCategory>> GetAllAsync(int categoryID);

		Task<List<ServiceCategory>> GetAllAsync(int clinicID, bool includeServices);

        Task<List<ServiceCategory>> GetAllActiveAsync(int clinicID, bool includeServices);

		Task<int> GetIDAsync(int serviceClinicDoctorID);
		
        Task<PaginatedList<ServiceCategory>> GetAllAsync(ServiceCategorySearch search);
    }
}