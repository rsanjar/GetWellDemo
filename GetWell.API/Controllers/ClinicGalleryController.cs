using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicGalleryController :  BaseApiController<ClinicGallery>
	{
		#region ctor

		private readonly IClinicGalleryService _service;

		public ClinicGalleryController(IClinicGalleryService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall/{clinicID:int}")]
        public async Task<ActionResult<List<ClinicGallery>>> GetAllAsync(int clinicID)
        {
            var result = await _service.GetAllAsync(clinicID);

            return Ok(result);
        }
	}
}