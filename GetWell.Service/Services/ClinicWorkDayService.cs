using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicWorkDayService : BaseService<ClinicWorkDay, Data.Model.ClinicWorkDay>, IClinicWorkDayService
	{
        #region ctor

        private readonly IRepository<Data.Model.ClinicWorkDay> _repository;

        public ClinicWorkDayService(IRepository<Data.Model.ClinicWorkDay> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<List<ClinicWorkDay>> GetAllAsync(int clinicId)
        {
            var query = _repository.Context.ClinicWorkDays
                .Where(c => c.ClinicID == clinicId)
                .OrderBy(c => c.WeekDayID);

            var result = await ToListAsync(query);

            return result;
        }
    }
}