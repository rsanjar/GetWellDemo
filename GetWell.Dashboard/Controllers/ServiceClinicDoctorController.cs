using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Dashboard.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
    public class ServiceClinicDoctorController : BaseController
    {
        #region ctor

        private readonly IServiceClinicDoctorService _service;
        private readonly IClinicDoctorService _clinicDoctorService;

        public ServiceClinicDoctorController(IServiceClinicDoctorService service, 
	        IClinicDoctorService clinicDoctorService)
        {
	        _service = service;
	        _clinicDoctorService = clinicDoctorService;
        }

        #endregion

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("clinic-doctor-services/{clinicId:int}/{doctorId:int}")]
        public async Task<IActionResult> Index(int clinicId, int doctorId)
        {
            if (clinicId <= 0 || doctorId <= 0)
                return RedirectToAction("Index", "Home");

            var result = await _service.GetAllByDoctor(clinicId, doctorId);

            if(result == null || result.Count == 0)
                return RedirectToAction("Index", "Home");

            var model = new ServiceClinicDoctorModel
            {
                ClinicID = clinicId,
                DoctorID = doctorId,
                ServiceCategories = result.Select(c => new {c.Service.ServiceCategoryID, c.Service.ServiceCategoryName})
                    .Distinct().ToDictionary(c => c.ServiceCategoryID, c => c.ServiceCategoryName),
                ServiceClinicDoctors = new Dictionary<int, List<ServiceClinicDoctor>>()
            };

            foreach (var c in model.ServiceCategories)
            {
                model.ServiceClinicDoctors.Add(c.Key, result.Where(f => f.Service.ServiceCategoryID == c.Key).ToList());
            }

            return View("Index", model);
        }

        [Authorize(Roles = UserRoles.Clinic)]
        [HttpGet("doctor-services/{doctorId:int}")]
        public async Task<IActionResult> DoctorServices(int doctorId)
        {
            return await Index(User.ID(), doctorId);
        }

        [Authorize(Roles = UserRoles.Doctor)]
        [HttpGet("clinic-doctor-services")]
        public async Task<IActionResult> MyServices()
        {
	        var item = await _clinicDoctorService.GetAsync(User.ID());

            if(item == null)
	            return RedirectToAction("Index", "Home");

            return await Index(item.ClinicID, item.DoctorID);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ServiceClinicDoctorModel model)
        {
            foreach (var item in model.ServiceClinicDoctors.FirstOrDefault().Value)
            {
                if (item.ID > 0)
                    await _service.UpdateAsync(item);
                else
                    await _service.SaveAsync(item);

                await _service.SaveOrUpdateAsync(item);
            }

            var result = await _service.GetAllByDoctor(model.ClinicID, model.DoctorID, model.CategoryID);

            model.ServiceClinicDoctors = new Dictionary<int, List<ServiceClinicDoctor>> {{model.CategoryID, result}};

            return PartialView("Cards/SaveFormData", model);
        }
    }
}