using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ServiceClinicController : BaseApiController<ServiceClinic>
	{
		#region ctor

		private readonly IServiceClinicService _service;

		public ServiceClinicController(IServiceClinicService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [AllowAnonymous]
        [HttpGet("getAll/{clinicID:int}")]
        public async Task<ActionResult<List<ServiceClinic>>> GetAll(int clinicID)
        {
            var result = await _service.GetAllAsync(clinicID);

            InitLocalization(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<ActionResult<List<ServiceClinic>>> GetAll(ServiceClinicSearch search)
        {
            search.CityID = search.CityID > 0 ? search.CityID : PatientCityID;
            
            var result = await _service.SearchAsync(search);
            
            InitLocalization(result);

	        return Ok(result);
        }
        
		[AllowAnonymous]
        [HttpPost("get-all-by-category")]
        public async Task<ActionResult<List<ServiceClinic>>> GetAllByServiceCategoryAsync(int clinicId, int serviceCategoryId)
        {
            var result = await _service.GetAllByServiceCategoryAsync(clinicId, serviceCategoryId, true);

            InitLocalization(result);

	        return Ok(result);
        }
    }
}