using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
	public class CountryController : BaseApiController<Country>
	{
		#region ctor

		private readonly ICountryService _service;

		public CountryController(ICountryService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

	}
}