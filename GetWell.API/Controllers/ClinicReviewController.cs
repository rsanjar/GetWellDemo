using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicReviewController : BaseApiController<ClinicReview>
	{
		#region ctor

		private readonly IClinicReviewService _service;

		public ClinicReviewController(IClinicReviewService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall/{clinicID:int}")]
        public async Task<ActionResult<List<ClinicReview>>> GetAllAsync(int clinicID, 
            int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
        {
            var result = await _service.GetAllAsync(clinicID, new PaginatedList<ClinicReview>(orderBy, pageNumber, pageSize, isAsc));

            return Ok(result);
        }

        [HttpGet("get-rating/{clinicID:int}")]
        public async Task<ActionResult<double>> GetClinicRatingAsync(int clinicID)
        {
            var result = await _service.GetRatingAsync(clinicID);

            return Ok(result);
        }

        [HttpGet("count/{clinicID:int}")]
        public async Task<ActionResult<double>> CountAsync(int clinicID)
        {
            var result = await _service.CountByClinicAsync(clinicID);

            return Ok(result);
        }
	}
}