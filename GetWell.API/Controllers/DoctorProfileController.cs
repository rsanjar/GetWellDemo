using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class DoctorProfileController : BaseApiController<DoctorProfile>
	{
		#region ctor

		private readonly IDoctorProfileService _service;

		public DoctorProfileController(IDoctorProfileService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}