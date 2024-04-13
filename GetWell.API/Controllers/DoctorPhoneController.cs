using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class DoctorPhoneController : BaseApiController<DoctorPhone>
	{
		#region ctor

		private readonly IDoctorPhoneService _service;

		public DoctorPhoneController(IDoctorPhoneService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}