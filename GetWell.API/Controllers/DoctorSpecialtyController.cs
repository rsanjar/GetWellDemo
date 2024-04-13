using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class DoctorSpecialtyController : BaseApiController<DoctorSpecialty>
	{
		#region ctor

		private readonly IDoctorSpecialtyService _service;

		public DoctorSpecialtyController(IDoctorSpecialtyService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}