using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class TitleController : BaseApiController<Title>
	{
		#region ctor

		private readonly ITitleService _service;

		public TitleController(ITitleService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}