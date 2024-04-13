using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
    public class ServiceController : BaseApiController<DTO.Service>
	{
		#region ctor

		private readonly IServiceService _service;
        private readonly IClinicService _clinicService;
        private readonly IDoctorService _doctorService;

        public ServiceController(IServiceService service, 
			IClinicService clinicService,
			IDoctorService doctorService,
            PatientCacheHelper patientCacheHelper) : 
            base(service, patientCacheHelper)
        {
            _service = service;
            _clinicService = clinicService;
            _doctorService = doctorService;
        }

		#endregion

		[AllowAnonymous]
		[HttpGet("getAll/{serviceCategoryID:int}")]
		public async Task<ActionResult<List<DTO.Service>>> GetAll(int serviceCategoryID)
		{
			return await _service.GetAllAsync(serviceCategoryID);
		}

		[AllowAnonymous]
		[HttpGet("getAll/{clinicID:int}/{serviceCategoryID:int}")]
		public async Task<ActionResult<List<DTO.Service>>> GetAll(int clinicID, int serviceCategoryID)
		{
			return await _service.GetAllAsync(clinicID, serviceCategoryID);
		}

		[AllowAnonymous]
		[HttpPost("search")]
		public async Task<ActionResult<PaginatedList<DTO.Service>>> Search(ServiceSearch search)
		{
			return await _service.GetAllAsync(search);
		}

        [AllowAnonymous]
        [HttpGet("autocomplete")]
        public async Task<ActionResult<GeneralSearchModel>> AutoComplete(string term, int pageSize = 5)
        {
            if (term.Length < 2)
                return Ok(new GeneralSearchModel());

            var serviceList = await _service.AutoCompleteSearchAsync(term, pageSize);
            var clinicList = await _clinicService.AutoCompleteSearchAsync(term, pageSize);
            //var doctorList = await _doctorService.AutoCompleteSearchAsync(term, pageSize);
            
			foreach(var clinic in clinicList)
				clinic.Init(PatientLanguage);

            // foreach(var doctor in doctorList)
            //     doctor.Init(PatientLanguage);

            InitLocalization(serviceList);

            var serviceKeyValue = serviceList.Select(c => new NameValueModel(c.ID, c.Name)).ToList();
            var clinicKeyValue = clinicList.Select(c => new NameValueModel(c.ID, c.Name)).ToList();
            //var doctorKeyValue = doctorList.Select(c => new NameValueModel(c.ID, c.FullName)).ToList();

            GeneralSearchModel result = new GeneralSearchModel()
            {
				Services = serviceKeyValue,
				Clinics = clinicKeyValue,
                //Doctors = doctorKeyValue
            };
            
            return Ok(result);
        }
	}
}