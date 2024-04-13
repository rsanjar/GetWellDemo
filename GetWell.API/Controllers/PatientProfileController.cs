using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class PatientProfileController : BaseApiController<PatientProfile>
	{
		#region ctor

		private readonly IPatientProfileService _service;

		public PatientProfileController(IPatientProfileService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}