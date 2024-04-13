using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class ClinicReviewService : BaseService<ClinicReview, Data.Model.ClinicReview>, IClinicReviewService
    {
        #region ctor

        private readonly IRepository<Data.Model.ClinicReview> _repository;

        public ClinicReviewService(IRepository<Data.Model.ClinicReview> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<PaginatedList<ClinicReview>> GetAllAsync(int clinicId, PaginatedList<ClinicReview> pagination)
        {
            var result = await _repository.Context.ClinicReviews
                .Where(c => c.ClinicID == clinicId && c.IsBlocked == false && c.IsDisabled == false)
                .GetPaginatedListAsync(pagination);

            return result;
        }

        public async Task<double> GetRatingAsync(int clinicId)
        {
            var result = await _repository.Entity
                .Where(c => c.ClinicID == clinicId)
                .AverageAsync(c => c.Rating);

            return result;
        }

        public async Task<int> CountByClinicAsync(int clinicId)
        {
            var result = await _repository.CountAsync(c => c.ClinicID == clinicId);

            return result;
        }
	}
}