using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class CityController :  BaseApiController<City>
	{
		#region ctor

		private readonly ICityService _service;

		public CityController(ICityService service, 
            PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [AllowAnonymous]
        [HttpGet("getall/{countryID:int}")]
        public async Task<ActionResult<List<City>>> GetAll(int countryID = 1)
        {
            var list = await _service.GetAllAsync(countryID);

			InitLocalization(list);

            return Ok(list);
        }
	}
}