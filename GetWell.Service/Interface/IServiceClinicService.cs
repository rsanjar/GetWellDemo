using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IServiceClinicService : IBaseService<ServiceClinic>
    {
        Task<List<ServiceClinic>> GetAllAsync(int clinicId);

        Task<List<ServiceClinic>> GetAllByClinicAsync(int clinicId, int? serviceCategoryId = null);

        Task<List<ServiceClinic>> GetAllByServiceCategoryAsync(int clinicId, int serviceCategoryId, bool getOnlyActive = false);

        Task<PaginatedList<ServiceClinic>> SearchAsync(ServiceClinicSearch search);

        Task<ServiceClinic> GetByClinicDoctorAsync(int serviceClinicDoctorID);

        Task<int> GetIDAsync(int serviceClinicDoctorID);
    }
}