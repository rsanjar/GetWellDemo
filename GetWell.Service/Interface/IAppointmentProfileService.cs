
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IAppointmentProfileService : IBaseService<DTO.AppointmentProfile>
    {
        Task<PaginatedList<AppointmentProfile>> SearchReviewAsync(ReviewSearch search);
    }
}