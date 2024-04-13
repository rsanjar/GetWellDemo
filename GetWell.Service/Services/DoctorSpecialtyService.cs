using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class DoctorSpecialtyService : BaseService<DoctorSpecialty, Data.Model.DoctorSpecialty>, IDoctorSpecialtyService
	{
		public DoctorSpecialtyService(IRepository<Data.Model.DoctorSpecialty> repository) : base(repository)
		{
		}
	}
}