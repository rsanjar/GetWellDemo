using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicGalleryService : BaseService<ClinicGallery, Data.Model.ClinicGallery>, IClinicGalleryService
    {
        #region ctor

        private readonly IRepository<Data.Model.ClinicGallery> _repository;

        public ClinicGalleryService(IRepository<Data.Model.ClinicGallery> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<ClinicGallery>> GetAllAsync(int clinicId)
        {
            var query = _repository.Entity
                .Where(c => c.ClinicID == clinicId)
                .OrderBy(c => c.SortOrder)
                .Select(c => c);

            var result = await ToListAsync(query);

            return result;
        }
    }
}