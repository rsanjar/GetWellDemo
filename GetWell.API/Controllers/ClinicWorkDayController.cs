using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicWorkDayController : BaseApiController<ClinicWorkDay>
	{
		#region ctor

		private readonly IClinicWorkDayService _service;

		public ClinicWorkDayController(IClinicWorkDayService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall/{clinicID:int}")]
        public async Task<ActionResult<List<ClinicWorkDay>>> GetAllAsync(int clinicID)
        {
            var result = await _service.GetAllAsync(clinicID);

            return Ok(result);
        }
	}
}