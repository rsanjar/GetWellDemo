using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ClinicDoctorWorkDayService : BaseService<ClinicDoctorWorkDay, Data.Model.ClinicDoctorWorkDay>, IClinicDoctorWorkDayService
	{
        #region ctor

        private readonly IRepository<Data.Model.ClinicDoctorWorkDay> _repository;

        public ClinicDoctorWorkDayService(IRepository<Data.Model.ClinicDoctorWorkDay> repository) : base(repository)
        {
            _repository = repository;
        }
        
        #endregion

        public async Task<List<ClinicDoctorWorkDay>> GetAllAsync(int clinicDoctorID)
        {
            var query = _repository.Entity
	            .Where(c => c.ClinicDoctorID == clinicDoctorID)
	            .OrderBy(c => c.WeekDayID);

            var result = await ToListAsync(query);

            return result;
        }

        public async Task<ClinicDoctorWorkDay> GetAsync(int clinicDoctorID, DateTime date)
        {
	        var weekDay = (int)date.DayOfWeek;

	        if (date.DayOfWeek == DayOfWeek.Sunday)
		        weekDay = 7;

	        var query = _repository.Entity
		        .Where(c => c.ClinicDoctorID == clinicDoctorID && c.WeekDayID == weekDay)
		        .OrderBy(c => c.WeekDayID);

	        var result = await FirstOrDefaultAsync(query);

	        return result;
        }

		public async Task<List<ClinicDoctorWorkDay>> GetAllAsync(int clinicID, int doctorID)
		{
			var query = from c in _repository.Entity 
                join k in _repository.Context.ClinicDoctors on c.ClinicDoctorID equals k.ID
				where k.ClinicID == clinicID && k.DoctorID == doctorID
				select c;

            var result = await ToListAsync(query);

			return result;
		}
    }
}