using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.AdminOrClinic)]
    public class ClinicController : BaseImageController
	{
        #region ctor

        private readonly IClinicService _service;
        private readonly IClinicPhoneService _clinicPhoneService;
        private readonly IClinicAccountService _clinicAccountService;
        private readonly IClinicWorkDayService _clinicWorkDayService;
        private readonly ICityService _cityService;
        private readonly FormCacheHelper _formCacheHelper;

        public ClinicController(IClinicService service,
            IClinicPhoneService clinicPhoneService,
            IClinicAccountService clinicAccountService,
            IClinicWorkDayService clinicWorkDayService,
            ICityService cityService,
            FormCacheHelper formCacheHelper, 
            IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _service = service;
            _clinicPhoneService = clinicPhoneService;
            _clinicAccountService = clinicAccountService;
            _clinicWorkDayService = clinicWorkDayService;
            _cityService = cityService;
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        [HttpGet("[controller]")]
        [Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View(new ClinicSearch()));
		}

		[HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> SearchResult(ClinicSearch search)
        {
            return await Task.FromResult(ViewComponent(typeof(ClinicSearchResultViewComponent), new { search }));
        }

        [HttpGet("[controller]/add")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Add()
        {
            var clinic = await GetClinic();
            
            return View("Save", clinic);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Add(Clinic item)
        {
            if(!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            item.UniqueKey = Guid.NewGuid();
            item.IsActive = item.Account.IsActive;
            
            var result = await _service.SaveAsync(item);

            if (result.IsSuccess)
            {
                await SaveImages(item);
                await _service.UpdateAsync(item);//saving image urls

                string qrImage = QrCodeHelper.GenerateQrCodeImage(item.UniqueKey.ToString());
                await _service.SaveQrImageBase64Async(item.ID, qrImage);
            }

            item.Account.ClinicID = item.ID;
            item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);
            //filter fields which are allowed to be inserted
            if (result.IsSuccess)
            {
                item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);

                result = await _clinicAccountService.SaveAsync(item.Account);
            }

            if (result.IsSuccess)
                result = await SaveWorkDays(item.WorkDay, item.ID);

            if (result.IsSuccess)
                result = await SavePhones(item.Phones, item.ID);

            if (!result.IsSuccess)
                await _service.DeleteAsync(item.ID); //cleanup

            return await ContentResultAsync(result.MessageKey);
        }
        
        [HttpGet("[controller]/edit/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await GetClinic(id);

            if (item == null)
                return RedirectToAction("Index", "Clinic");
            
            return View("Save", item);
        }

        [HttpGet("[controller]/profile")]
        [Authorize(Roles = UserRoles.Clinic)]
        public async Task<IActionResult> Profile()
        {
            var item = await GetClinic(User.ID());

            if (item == null)
                return RedirectToAction("Index", "Home");

            return View("Save", item);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> Edit(Clinic item)
        {
            ModelState.Remove("Account.Password");
            ModelState.Remove("Account.ConfirmPassword");

            //Allow a clinic to edit only their own account
            if (User.IsInRole(UserRoles.Clinic) && item.ID != User.ID())
                return await ContentResultAsync(Crud.AccessDeniedError);

            if (!ModelState.IsValid || item.ID <= 0)
                return await ContentResultAsync(Crud.ValidationError);

            var clinic = await GetClinic(item.ID);

            if (clinic == null)
                return await ContentResultAsync(Crud.ItemNotFoundError);

            await SaveImages(item);

            var result = await _service.UpdateAsync(clinic.InitUpdate(item));

            if (result.IsSuccess && User.IsInRole(UserRoles.Admin))
            {
                item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);

                result = await _clinicAccountService.UpdateAsync(clinic.Account.InitUpdate(item.Account));
            }
                
            if(result.IsSuccess)
                result = await SaveWorkDays(item.WorkDay, clinic.ID);

            if (result.IsSuccess)
                result = await SavePhones(item.Phones, item.ID);
            
            return await ContentResultAsync(result.MessageKey);
        }
        
        #region Private Methods

        private async Task SaveImages(Clinic item)
        {
            item.LogoUrl = await SavePngImage(item.ID, item.LogoImage, new Size(250, 250), ImageFolderEnum.ClinicLogo);
            item.ThumbnailUrl = await SavePngImage(item.ID, item.ThumbnailImage, new Size(700, 700), ImageFolderEnum.ClinicThumbnail);
        }

        private async Task<Clinic> GetClinic(int id = 0)
        {
            Clinic clinic = null;

            if (id > 0)
                clinic = await _service.GetAsync(id);

            if (clinic != null)
            {
                var clinicCity = await _cityService.GetByRegionAsync(clinic.RegionID);
                clinic.CityID = clinicCity?.ID ?? 0;
                clinic.Phones = await _clinicPhoneService.GetAllAsync(id);
                clinic.WorkDay = await _clinicWorkDayService.GetAllAsync(id);
                clinic.Account = await _clinicAccountService.GetByClinicAsync(id);
                clinic.Cities = await _formCacheHelper.GetCitiesAsync(clinic.CityID);
            }
            else
            {
                clinic = new Clinic
                {
                    Account = new ClinicAccount(),
                    Phones = new List<ClinicPhone>(),
                    Cities = await _formCacheHelper.GetCitiesAsync()
                };
            }

            clinic.WorkDay = InitWorkDay(clinic.WorkDay);
            clinic.Phones = InitPhones(clinic.Phones);
            
            return clinic;
        }

        private List<ClinicWorkDay> InitWorkDay(List<ClinicWorkDay> items)
        {
            items ??= new List<ClinicWorkDay>();

            items.ForEach(c =>
            {
                c.OpenTimeNullable = c.OpenTime;
                c.CloseTimeNullable = c.CloseTime;
            });

            for (int i = 1; i <= 7; i++)
            {
                if (items.All(c => c.WeekDayID != i))
                    items.Add(new() {WeekDayID = i});
            }

            items = items.OrderBy(c => c.WeekDayID).ToList();

            return items;
        }

        private List<ClinicPhone> InitPhones(List<ClinicPhone> items)
        {
            items ??= new List<ClinicPhone>();
            int count = 5 - items.Count;

            for (int i = 1; i <= count; i++)
            {
                items.Add(new ClinicPhone());
            }

            return items;
        }

        private async Task<CrudResponse> SaveWorkDays(List<ClinicWorkDay> items, int clinicID)
        {
            var result = new CrudResponse(Crud.Success);

            foreach (var w in items ?? new List<ClinicWorkDay>())
            {
                if (w.OpenTimeNullable != null && w.CloseTimeNullable != null)
                {
                    w.ClinicID = clinicID;

                    result = await _clinicWorkDayService.SaveOrUpdateAsync(w);
                }

                if (w.ID > 0 && (w.OpenTimeNullable == null || w.CloseTimeNullable == null))
                {
                    result = await _clinicWorkDayService.DeleteAsync(w.ID);
                }
            }

            return result;
        }

        private async Task<CrudResponse> SavePhones(List<ClinicPhone> items, int clinicID)
        {
            var result = new CrudResponse(Crud.Success);

            foreach (var w in items ?? new List<ClinicPhone>())
            {
                if (w.ID > 0 && string.IsNullOrWhiteSpace(w.Phone))
                {
                    result = await _clinicPhoneService.DeleteAsync(w.ID);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(w.Phone))
                    {
                        w.Phone = BaseHelpers.CleanPhone(w.Phone);
                        w.ClinicID = clinicID;
                        w.IsMobile = true;
                        w.IsDisabled = false;

                        result = await _clinicPhoneService.SaveOrUpdateAsync(w);
                    }
                }
            }

            return result;
        }
        
        #endregion
    }
}