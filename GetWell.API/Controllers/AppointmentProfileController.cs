using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{

    [Route("api/review")]
    [ApiController]
	public class AppointmentProfileController :  BaseApiController<AppointmentProfile>
	{
		#region ctor
		
		private readonly IAppointmentProfileService _service;

		public AppointmentProfileController(IAppointmentProfileService service,
            PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<ActionResult<PaginatedList<AppointmentProfile>>> SearchReviews(ReviewSearch search)
        {
            var result = await _service.SearchReviewAsync(search);
			
            InitLocalization(result);

            return Ok(result);
        }
	}
}