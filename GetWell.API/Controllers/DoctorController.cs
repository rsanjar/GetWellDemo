using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class DoctorController : BaseApiController<Doctor>
	{
		#region ctor

		private readonly IDoctorService _service;

		public DoctorController(IDoctorService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

		[HttpGet("getall/{clinicID:int}")]
		public async Task<ActionResult<List<Doctor>>> GetAllAsync(int clinicID)
		{
			var result = await _service.GetAllAsync(clinicID);

			InitLocalization(result);

			return Ok(result);
		}
	}
}