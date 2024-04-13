using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers
{         

    [Authorize(Roles = UserRoles.Admin)]
    public class PatientController : BaseController
    {
        #region ctor

        private readonly IPatientService _service;
        private readonly IPatientProfileService _patientProfileService;
        private readonly IPatientAccountService _patientAccountService;
        private readonly ICityService _cityService;
        private readonly FormCacheHelper _formCacheHelper;

        public PatientController(IPatientService service, 
            IPatientProfileService patientProfileService,
            IPatientAccountService patientAccountService,
            ICityService cityService,
            FormCacheHelper formCacheHelper)
        {
            _service = service;
            _patientProfileService = patientProfileService;
            _patientAccountService = patientAccountService;
            _cityService = cityService;
            _formCacheHelper = formCacheHelper;
        }

        #endregion
        
        [HttpGet("[controller]")]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View(new PatientSearch(){ ClinicID = User.ID()}));
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(PatientSearch search)
        {
            return await Task.FromResult(ViewComponent(typeof(PatientSearchResultViewComponent), new {search}));
        }
        
        [HttpGet("[controller]/add")]
        public async Task<IActionResult> Add()
        {
            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var item = await GetPatientAsync();

            return View("Save", item);
        }

        [HttpGet("[controller]/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await GetPatientAsync(id);
            
            return View("Save", item);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(Patient item)
        {
            var result = await _service.SaveAsync(item);

            if (result.IsSuccess)
            {
                item.PatientProfile.PatientID = item.ID;
                result = await _patientProfileService.SaveAsync(item.PatientProfile);
            }

            if(result.IsSuccess)
                await _patientAccountService.SaveAsync(item.PatientAccount.InitSave(item.PatientAccount));

            return await ContentResultAsync(result.MessageKey);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Patient item)
        {
            var result = await _service.UpdateAsync(item);
            await _patientProfileService.UpdateAsync(item.PatientProfile);
            var account = await _patientAccountService.GetAsync(item.PatientAccount.ID);
            await _patientAccountService.UpdateAsync(account.InitUpdate(item.PatientAccount));

            return await ContentResultAsync(result.MessageKey);
        }

        #region private methods

        private async Task<Patient> GetPatientAsync(int id = 0)
        {
            var item = id > 0 ? await _service.GetAsync(id) : new Patient();

            if (id > 0)
            {
                item.PatientProfile = await _patientProfileService.GetByPatientAsync(id);
                item.PatientAccount = await _patientAccountService.GetByPatientIDAsync(id);
            }
            else
            {
                item.PatientProfile = new PatientProfile();
                item.PatientAccount = new PatientAccount();
            }
            
            item.Languages = await _formCacheHelper.GetLanguagesAsync();

            int regionID = item.PatientProfile.RegionID.GetValueOrDefault(0);

            if (regionID > 0)
            {
                var city = await _cityService.GetByRegionAsync(regionID);

                if (city != null)
                    item.PatientProfile.CityID = city.ID;
            }
            
            item.PatientProfile.Cities = await _formCacheHelper.GetCitiesAsync(item.PatientProfile.CityID);

            return item;
        }

        #endregion
    }
}