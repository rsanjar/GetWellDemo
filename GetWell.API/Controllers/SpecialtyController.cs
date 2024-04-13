using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class SpecialtyController : BaseApiController<Specialty>
	{
		#region ctor

		private readonly ISpecialtyService _service;

		public SpecialtyController(ISpecialtyService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}