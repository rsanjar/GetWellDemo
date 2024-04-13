using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class RegionController : BaseApiController<Region>
	{
		#region ctor

		private readonly IRegionService _service;

		public RegionController(IRegionService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

		[AllowAnonymous]
		[HttpGet("getall/{cityID}")]
		public async Task<ActionResult<List<Region>>> GetAll(int cityID)
		{
			var result = await _service.GetAllAsync(cityID);

			return Ok(result);
		}
	}
}