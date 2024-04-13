using System;
using System.Collections.Generic;
using System.IO;
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
    [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
    public class DoctorController : BaseController
    {
        #region ctor

        private readonly IDoctorService _service;
        private readonly IDoctorPhoneService _doctorPhoneService;
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly IClinicDoctorAccountService _clinicDoctorAccountService;
        private readonly IClinicDoctorWorkDayService _clinicDoctorWorkDayService;
        private readonly IClinicDoctorService _clinicDoctorService;
        private readonly IClinicService _clinicService;
        private readonly IClinicAccountService _clinicAccountService;
        private readonly IServiceClinicService _serviceClinicService;
        private readonly IServiceClinicDoctorService _serviceClinicDoctorService;
        private readonly ICityService _cityService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FormCacheHelper _formCacheHelper;

        public DoctorController(IDoctorService service,
            IDoctorPhoneService doctorPhoneService,
            IDoctorProfileService doctorProfileService,
            IClinicDoctorAccountService clinicDoctorAccountService,
            IClinicDoctorWorkDayService clinicDoctorWorkDayService,
            IClinicDoctorService clinicDoctorService,
            ICityService cityService,
            IWebHostEnvironment webHostEnvironment,
            FormCacheHelper formCacheHelper,
            IClinicService clinicService,
            IClinicAccountService clinicAccountService,
            IServiceClinicService serviceClinicService,
            IServiceClinicDoctorService serviceClinicDoctorService)
        {
            _service = service;
            _doctorPhoneService = doctorPhoneService;
            _doctorProfileService = doctorProfileService;
            _clinicDoctorAccountService = clinicDoctorAccountService;
            _clinicDoctorWorkDayService = clinicDoctorWorkDayService;
            _clinicDoctorService = clinicDoctorService;
            _cityService = cityService;
            _webHostEnvironment = webHostEnvironment;
            _formCacheHelper = formCacheHelper;
            _clinicService = clinicService;
            _clinicAccountService = clinicAccountService;
            _serviceClinicService = serviceClinicService;
            _serviceClinicDoctorService = serviceClinicDoctorService;
        }

        #endregion

        [HttpGet("[controller]")]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> Index()
        {
            //await SaveSampleDoctor(13);

            return await Task.FromResult(View(new DoctorSearch() { ClinicID = User.ID() }));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> SearchResult(DoctorSearch search)
        {
            if (User.IsInRole(UserRoles.Clinic))
                search.ClinicID = User.ID();

            return await Task.FromResult(ViewComponent(typeof(DoctorSearchResultViewComponent), new { search }));
        }

        [HttpGet("clinic/{clinicID:int}/[controller]/add", Order = 1)]
        [HttpGet("[controller]/add", Order = 2)]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> Add(int clinicID = 0)
        {
            var item = await GetDoctor(0, User.ID(clinicID));

            return View("Save", item);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> AddNew(Doctor item)
        {
            bool isExists = await _clinicDoctorAccountService.IsAnExistingUserAsync(item.Account.MobilePhone);

            if (isExists)
                return await ContentResultAsync(Crud.DuplicateEntryError);


            //Clinics can add doctors only to themselves
            if (User.IsInRole(UserRoles.Clinic) && User.ID() != item.ClinicID)
                return await ContentResultAsync(Crud.AccessDeniedError);

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var result = await _service.SaveAsync(item);

            if (result.IsSuccess)
            {
                item.Profile.DoctorID = item.ID;

                await SaveImage(item.Profile);
            }

            if (result.IsSuccess)
                result = await _doctorProfileService.SaveAsync(item.Profile);


            if (item.ClinicID > 0)
            {
                var clinicDoctor = new ClinicDoctor
                {
                    ClinicID = item.ClinicID.GetValueOrDefault(0),
                    DoctorID = item.ID,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow,
                    Doctor = null,
                    DoctorProfile = null,
                    DateDisabled = null
                };

                if (result.IsSuccess)
                    result = await _clinicDoctorService.SaveAsync(clinicDoctor);

                if (result.IsSuccess)
                {
                    item.Account.ClinicDoctorID = clinicDoctor.ID;
                    item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);

                    try
                    {
                        result = await _clinicDoctorAccountService.SaveAsync(item.Account.InitSave(item.Account));
                    }
                    catch
                    {
                        result = new CrudResponse(Crud.Error);
                    }
                }

                if (result.IsSuccess)
                    result = await SaveWorkDays(item.WorkDay, item.Account.ClinicDoctorID);
            }

            if (result.IsSuccess)
                result = await SavePhones(item.Phones, item.ID);

            if (!result.IsSuccess)
                await _service.DeleteAsync(item.ID); //cleanup

            return await ContentResultAsync(result.MessageKey);
        }

        [HttpGet("clinic/{clinicID:int}/[controller]/edit/{id:int}", Order = 1)]
        [HttpGet("[controller]/edit/{id:int}", Order = 2)]
        [Authorize(Roles = UserRoles.AdminOrClinic)]
        public async Task<IActionResult> Edit(int id, int clinicID = 0)
        {
            var item = await GetDoctor(id, User.ID(clinicID));

            return View("Save", item);
        }

        [HttpGet("[controller]/profile")]
        [Authorize(Roles = UserRoles.Doctor)]
        public async Task<IActionResult> Profile()
        {
            var item = await GetDoctor(User.ID());

            if (item == null)
                return RedirectToAction("Index", "Home");

            return View("Save", item);
        }

        [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
        [HttpPost]
        public async Task<IActionResult> Save(Doctor item)
        {
            //Doctor can edit only their own account
            if (User.IsInRole(UserRoles.Doctor) && item.Account.ClinicDoctorID != User.ID())
                return await ContentResultAsync(Crud.AccessDeniedError);

            //Clinic can edit only their own doctor
            if (User.IsInRole(UserRoles.Clinic) && item.ClinicID != User.ID())
                return await ContentResultAsync(Crud.AccessDeniedError);

            if (item.Account != null && item.Account.ClinicDoctorID > 0)
            {
                ModelState.Remove($"{nameof(Account)}.{nameof(Account.Password)}");
                ModelState.Remove($"{nameof(Account)}.{nameof(ClinicAccount.ConfirmPassword)}");
            }

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var doctor = await GetDoctor(item.ID, item.ClinicID.GetValueOrDefault(0));
            var result = await _service.UpdateAsync(doctor.InitUpdate(item));

            item.Profile.DoctorID = doctor.ID;

            if (!await SaveImage(item.Profile))
                return await ContentResultAsync(Crud.Error);

            if (result.IsSuccess)
                result = await _doctorProfileService.UpdateAsync(doctor.Profile.InitUpdate(item.Profile));

            if (item.ClinicID > 0 && item.Account != null)
            {
                if (result.IsSuccess && (User.IsInRole(UserRoles.Admin) || User.IsInRole(UserRoles.Clinic)))
                {
                    if (item.Account.ID > 0)
                    {
                        item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);
                        result = await _clinicDoctorAccountService.UpdateAsync(doctor.Account.InitUpdate(item.Account));
                    }
                    else
                        result = await SaveClinicDoctorAccount(item);
                }

                if (result.IsSuccess)
                    result = await SaveWorkDays(item.WorkDay, item.Account.ClinicDoctorID);
            }

            if (result.IsSuccess)
                result = await SavePhones(item.Phones, item.ID);

            return await ContentResultAsync(result.MessageKey);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateReceptionDoctorForAllClinics()
        {
            var list = await _clinicService.GetAllAsync(new ClinicSearch()
            {
                PageSize = 99998
            });

            foreach (var i in list)
            {
                await SaveDoctorWithAllServices(i.ID);
            }

            return Content("success");
        }

        #region Private Methods

        private async Task<CrudResponse> SaveDoctorWithAllServices(int clinicID)
        {
            var clinicAccount = await _clinicAccountService.GetByClinicAsync(clinicID);

            var accountID = await _clinicDoctorAccountService.GetAccountID(clinicAccount.MobilePhone);

            if (accountID > 0)
                return new CrudResponse(Crud.DuplicateEntryError);

            var search = await _service.GetAllAsync(new DoctorSearch()
            {
                FirstName = "Администрация",
                LastName = "Администрация",
                MiddleName = "Администрация"
            });

            if (search.Any())
                return new CrudResponse(Crud.DuplicateEntryError);

            var clinic = await _clinicService.GetAsync(clinicID);

            var item = new Doctor
            {
                FirstName = "Клиники",
                LastName = "Администрация",
                MiddleName = clinic.Name.Length > 50 ? clinic.Name.Substring(0, 50) : clinic.Name,
                Email = "getwell.uz@gmail.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-40),
                CareerStartDate = DateTime.UtcNow.AddYears(-10),
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                ClinicID = clinic.ID,
                ClinicName = clinic.Name,
                IsFamilyDoctor = false,
                IsRetired = false,

                Phones = new List<DoctorPhone>(),
                Profile = new DoctorProfile(),

                Account = new ClinicDoctorAccount(),
                WorkDay = new List<ClinicDoctorWorkDay>()
            };

            var response = await _service.SaveAsync(item);


            if (response.IsSuccess)
            {
                item.Profile.DoctorID = item.ID;
                item.Profile.PreferredLanguageID = 0;
                item.Profile.RegionID = clinic.RegionID;
                item.Profile.IsMale = true;


                await SaveImage(item.Profile);
            }

            if (response.IsSuccess)
                response = await _doctorProfileService.SaveAsync(item.Profile);


            if (item.ClinicID > 0)
            {
                var clinicDoctor = new ClinicDoctor
                {
                    ClinicID = item.ClinicID.GetValueOrDefault(0),
                    DoctorID = item.ID,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow,
                    Doctor = null,
                    DoctorProfile = null,
                    DateDisabled = null
                };

                if (response.IsSuccess)
                    response = await _clinicDoctorService.SaveAsync(clinicDoctor);



                if (response.IsSuccess)
                {
                    item.Account.ClinicDoctorID = clinicDoctor.ID;
                    item.Account.IsActive = true;
                    item.Account.DateCreated = DateTime.UtcNow;
                    item.Account.MobilePhone = BaseHelpers.CleanPhone(clinicAccount.MobilePhone);
                    item.Account.Password = clinicAccount.Password;
                    item.Account.Email = clinicAccount.Email;
                    item.Account.SmsActivationCode = clinicAccount.SmsActivationCode;

                    response = await _clinicDoctorAccountService.SaveAsync(item.Account.InitSave(item.Account));
                }


                for (int i = 1; i <= 7; i++)
                {
                    if (item.WorkDay.All(c => c.WeekDayID != i))
                        item.WorkDay.Add(new()
                        {
                            WeekDayID = i,
                            ClinicDoctorID = clinicDoctor.ID,
                            StartTime = new TimeSpan(9, 0, 0),
                            EndTime = new TimeSpan(23, 0, 0)
                        });
                }

                item.WorkDay.ForEach(c =>
                {
                    c.OpenTimeNullable = c.StartTime;
                    c.CloseTimeNullable = c.EndTime;
                });

                item.WorkDay = item.WorkDay.OrderBy(c => c.WeekDayID).ToList();

                item.Phones = new List<DoctorPhone>
                {
                    new()
                    {
                        IsPrimary = true,
                        Phone = clinicAccount.MobilePhone,
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        DoctorID = item.ID
                    }
                };


                if (response.IsSuccess)
                    response = await SaveWorkDays(item.WorkDay, item.Account.ClinicDoctorID);
            }

            if (response.IsSuccess)
                response = await SavePhones(item.Phones, item.ID);

            var clinicServices = await _serviceClinicService.GetAllAsync(clinic.ID);
            var clinicDoctorServices = new List<ServiceClinicDoctor>();

            foreach (var service in clinicServices)
            {
                clinicDoctorServices.Add(new ServiceClinicDoctor
                {
                    ClinicDoctorID = item.Account.ClinicDoctorID,
                    ServiceClinicID = service.ID,
                    IsActive = true,
                    AverageDuration = service.AverageDuration,
                    Price = service.Price,
                    DateCreated = DateTime.UtcNow
                });
            }

            foreach (var a in clinicDoctorServices)
            {
                await _serviceClinicDoctorService.SaveOrUpdateAsync(a);
            }

            //response = await _serviceClinicDoctorService.SaveAllAsync(clinicDoctorServices);

            return response;
        }

        private async Task<Doctor> GetDoctor(int clinicDoctorID)
        {
            var result = await _clinicDoctorService.GetAsync(clinicDoctorID);

            return await GetDoctor(result.DoctorID, result.ClinicID);
        }

        private async Task<Doctor> GetDoctor(int doctorID, int clinicID)
        {
            Doctor item = null;

            if (doctorID > 0)
            {
                item = await _service.GetAsync(doctorID);
            }

            if (item != null)
            {
                item.Profile = await _doctorProfileService.GetByDoctorAsync(doctorID);
                var itemCity = await _cityService.GetByRegionAsync(item.Profile.RegionID);
                item.Profile.Cities = await _formCacheHelper.GetCitiesAsync(item.Profile.CityID);
                item.Profile.CityID = itemCity?.ID ?? 0;
                item.Phones = await _doctorPhoneService.GetAllAsync(doctorID);

                if (clinicID > 0)
                {
                    item.ClinicID = clinicID;
                    item.WorkDay = await _clinicDoctorWorkDayService.GetAllAsync(clinicID, doctorID);
                    item.Account = await _clinicDoctorAccountService.GetByClinicDoctorAsync(clinicID, doctorID);
                    item.WorkDay = InitWorkDay(item.WorkDay);
                }
            }
            else
            {
                item = new Doctor
                {
                    Phones = new List<DoctorPhone>(),
                    Profile = new DoctorProfile()
                    {
                        Cities = await _formCacheHelper.GetCitiesAsync()
                    },
                    ClinicID = clinicID,
                    Account = new ClinicDoctorAccount(),
                    WorkDay = InitWorkDay(new List<ClinicDoctorWorkDay>())
                };
            }

            if (clinicID > 0)
            {
                var clinic = await _clinicService.GetAsync(clinicID);

                item.ClinicName = clinic?.Name;
            }

            item.Phones = InitPhones(item.Phones);

            return item;
        }



        private async Task<CrudResponse> SaveClinicDoctorAccount(Doctor item)
        {
            CrudResponse result = new CrudResponse(Crud.Success);

            var clinicDoctor = await _clinicDoctorService.GetAsync(item.ClinicID.GetValueOrDefault(0), item.ID);

            if (clinicDoctor == null)
            {
                clinicDoctor = new ClinicDoctor
                {
                    ClinicID = item.ClinicID.GetValueOrDefault(0),
                    DoctorID = item.ID
                };

                result = await _clinicDoctorService.SaveAsync(clinicDoctor);
            }

            if (result.IsSuccess)
            {
                item.Account.ClinicDoctorID = clinicDoctor.ID;
                item.Account.MobilePhone = BaseHelpers.CleanPhone(item.Account.MobilePhone);

                result = await _clinicDoctorAccountService.SaveAsync(item.Account.InitSave(item.Account));
            }

            return result;
        }

        private List<ClinicDoctorWorkDay> InitWorkDay(List<ClinicDoctorWorkDay> items)
        {
            items ??= new List<ClinicDoctorWorkDay>();

            items.ForEach(c =>
            {
                c.OpenTimeNullable = c.StartTime;
                c.CloseTimeNullable = c.EndTime;
            });

            for (int i = 1; i <= 7; i++)
            {
                if (items.All(c => c.WeekDayID != i))
                    items.Add(new() { WeekDayID = i });
            }

            items = items.OrderBy(c => c.WeekDayID).ToList();

            return items;
        }

        private List<DoctorPhone> InitPhones(List<DoctorPhone> items)
        {
            items ??= new List<DoctorPhone>();
            int count = 5 - items.Count;

            for (int i = 1; i <= count; i++)
            {
                items.Add(new DoctorPhone());
            }

            return items;
        }

        private async Task<CrudResponse> SaveWorkDays(List<ClinicDoctorWorkDay> items, int clinicDoctorID)
        {
            var result = new CrudResponse(Crud.Success);

            foreach (var w in items)
            {
                if (w.BreakStartTime == null || w.BreakEndTime == null)
                {
                    w.BreakStartTime = null;
                    w.BreakEndTime = null;
                }

                if (w.OpenTimeNullable != null && w.CloseTimeNullable != null)
                {
                    w.ClinicDoctorID = clinicDoctorID;

                    result = await _clinicDoctorWorkDayService.SaveOrUpdateAsync(w);
                }

                if (w.ID > 0 && (w.OpenTimeNullable == null || w.CloseTimeNullable == null))
                {
                    result = await _clinicDoctorWorkDayService.DeleteAsync(w.ID);
                }
            }

            return result;
        }

        private async Task<CrudResponse> SavePhones(List<DoctorPhone> items, int doctorID)
        {
            var result = new CrudResponse(Crud.Success);

            foreach (var w in items)
            {
                if (w.ID > 0 && string.IsNullOrWhiteSpace(w.Phone))
                {
                    result = await _doctorPhoneService.DeleteAsync(w.ID);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(w.Phone))
                    {
                        w.Phone = BaseHelpers.CleanPhone(w.Phone);
                        w.DoctorID = doctorID;
                        w.IsMobile = true;
                        w.IsWorkPhone = true;
                        w.IsPrimary = w.IsPrimary;
                        w.IsActive ??= true;

                        if (w.ID > 0)
                            w.DateUpdated = DateTime.UtcNow;

                        result = await _doctorPhoneService.SaveOrUpdateAsync(w);
                    }
                }
            }

            return result;
        }

        private string GetImageFilePath(DoctorProfile item)
        {
            string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets\\images\\doctors");
            var fileName = string.Empty;

            if (item.DoctorID <= 0 || item.ProfilePicture == null)
                return fileName;

            fileName = $"{item.DoctorID}{Path.GetExtension(item.ProfilePicture.FileName)}".ToLower();
            item.ProfilePictureUrl = $"{BaseHelpers.GetBaseUrl}/assets/images/doctors/{fileName}";

            string filePath = Path.Combine(imageFolder, fileName);

            return filePath;
        }

        private async Task<bool> SaveImage(DoctorProfile item)
        {
            string filePath = GetImageFilePath(item);

            //profile picture is not a required field
            if (string.IsNullOrWhiteSpace(filePath))
                return true;

            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await item.ProfilePicture.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}