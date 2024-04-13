using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicPhoneController : BaseApiController<ClinicPhone>
	{
		#region ctor

		private readonly IClinicPhoneService _service;

		public ClinicPhoneController(IClinicPhoneService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall/{clinicID:int}")]
        public async Task<ActionResult<List<ClinicPhone>>> GetAllAsync(int clinicID)
        {
            var result = await _service.GetAllAsync(clinicID);

            return Ok(result);
        }
	}
}