using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ServiceCategoryController : BaseApiController<ServiceCategory>
	{
		#region ctor

		private readonly IServiceCategoryService _service;

		public ServiceCategoryController(IServiceCategoryService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

		[HttpGet("getall/{categoryID:int}")]
		public async Task<ActionResult<List<ServiceCategory>>> GetAll(int categoryID)
		{
			var result = await _service.GetAllAsync(categoryID);

			return Ok(result);
		}

		[HttpGet("getallByClinic/{clinicID:int}")]
		public async Task<ActionResult<List<ServiceCategory>>> GetAllByClinic(int clinicID, bool includeServices = false)
		{
			var result = await _service.GetAllActiveAsync(clinicID, includeServices);

			return Ok(result);
		}
	}
}