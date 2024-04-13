using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IDoctorService : IBaseService<Doctor>
	{
		Task<List<Doctor>> GetAllAsync(int clinicID);

        Task<List<Doctor>> GetAllAsync(DoctorSearch search);

        Task<PaginatedList<Doctor>> AutoCompleteSearchAsync(string term, int pageSize = 5);
    }
}