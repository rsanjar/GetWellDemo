using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class ZipCodeController :  BaseApiController<ZipCode>
	{
		#region ctor

		private readonly IZipCodeService _service;

		public ZipCodeController(IZipCodeService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion
	}
}