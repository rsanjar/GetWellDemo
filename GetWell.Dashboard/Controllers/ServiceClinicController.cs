using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Dashboard.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.AdminOrClinic)]
    public class ServiceClinicController : BaseController
    {
        #region ctor

        private readonly IServiceClinicService _service;

        public ServiceClinicController(IServiceClinicService service)
        {
            _service = service;
        }

        #endregion

        [HttpGet("[controller]")]
        [HttpGet("[controller]/{clinicId:int}")]
        public async Task<IActionResult> Index(int clinicId)
        {
            clinicId = User.ID(clinicId);

            if (clinicId <= 0)
                return RedirectToAction("Index", "Home");

            var result = await _service.GetAllByClinicAsync(clinicId);

            var model = new ServiceClinicModel
            {
                ClinicID = clinicId,
                ServiceCategories = result.Select(c => new {c.Service.ServiceCategoryID, c.Service.ServiceCategoryName})
                    .Distinct().ToDictionary(c => c.ServiceCategoryID, c => c.ServiceCategoryName),
                ServiceClinics = new Dictionary<int, List<ServiceClinic>>()
            };

            foreach (var c in model.ServiceCategories)
            {
                model.ServiceClinics.Add(c.Key, result.Where(f => f.Service.ServiceCategoryID == c.Key).ToList());
            }

            return View(model);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 1000000000)]
        public async Task<IActionResult> Save(ServiceClinicModel model)
        {
            foreach (var serviceClinic in model.ServiceClinics.FirstOrDefault().Value)
            {
                if(serviceClinic.ID > 0)
                    serviceClinic.DateUpdated = DateTime.UtcNow;
                else
                    serviceClinic.DateCreated = DateTime.UtcNow;
                
                await _service.SaveOrUpdateAsync(serviceClinic);
            }

            var result = await _service.GetAllByClinicAsync(model.ClinicID, model.CategoryID);

            model.ServiceClinics = new Dictionary<int, List<ServiceClinic>> { { model.CategoryID, result } };
            
            return PartialView("Cards/SaveFormData", model);
        }
    }
}