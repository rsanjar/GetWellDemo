using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GetWell.API.Controllers
{
	public class ClinicDoctorController : BaseApiController<ClinicDoctor>
	{
		#region ctor

		private readonly IClinicDoctorService _service;

		public ClinicDoctorController(IClinicDoctorService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall/{clinicID:int}")]
        public async Task<ActionResult<List<ClinicDoctor>>> GetAllAsync(int clinicID)
        {
            var result = await _service.GetAllAsync(clinicID);
            
            return Ok(result);
        }

        [HttpGet("get-all-by-service/{serviceClinicID:int}")]
        public async Task<ActionResult<List<ClinicDoctor>>> GetAllByServiceAsync(int serviceClinicID)
        {
	        int patientID = User.IsInRole(UserRoles.Patient) ? User.ID() : 0;

	        var result = await _service.GetAllByServiceAsync(serviceClinicID, patientID);

	        return Ok(result);
        }

        [HttpGet("get-all-key-value/{serviceClinicID:int}")]
        public async Task<ActionResult<NameValueModel>> GetAllByServiceBasicAsync(int serviceClinicID)
        {
            var list = await _service.GetAllByServiceBasicAsync(serviceClinicID);

            InitLocalization(list);

            list.ForEach(c => c.Doctor.Init(PatientLanguage));

            var result = list.Select(c => new NameValueModel(c.ID, c.Doctor.FullName));
            
            return Ok(result);
        }

        [HttpGet("get-all-by-clinic-key-value/{clinicID:int}")]
        public async Task<ActionResult<NameValueModel>> GetAllByClinicBasicAsync(int clinicID)
        {
            var list = await _service.GetAllAsync(clinicID);

            InitLocalization(list);

            list.ForEach(c => c.Doctor.Init(PatientLanguage));

            var result = list.Where(c => c.IsActive == true).Select(c => new NameValueModel(c.ID, c.Doctor.FullName));
            
            return Ok(result);
        }

        [HttpGet("get-all-by-service")]
        public async Task<ActionResult<List<ClinicDoctor>>> GetAllByClinicAndServiceAsync(int clinicID, int serviceID)
        {
	        var result = await _service.GetAllAsync(clinicID, serviceID);

	        return Ok(result);
        }

        [HttpGet("get-all-by-service-category")]
        public async Task<ActionResult<List<ClinicDoctor>>> GetAllByServiceCategoryAsync(int clinicID, int serviceCategoryID)
        {
	        var result = await _service.GetAllByServiceCategoryAsync(clinicID, serviceCategoryID);

	        return Ok(result);
        }
	}
}