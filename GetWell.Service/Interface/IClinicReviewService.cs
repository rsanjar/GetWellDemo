using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicReviewService : IBaseService<ClinicReview>
    {
        Task<PaginatedList<ClinicReview>> GetAllAsync(int clinicId, PaginatedList<ClinicReview> pagination);

        Task<double> GetRatingAsync(int clinicId);

        Task<int> CountByClinicAsync(int clinicId);
    }
}