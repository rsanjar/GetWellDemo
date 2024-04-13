using System;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
    public class AppointmentController : BaseController
    {
        #region ctor

        private readonly IAppointmentService _service;
        private readonly IAppointmentProfileService _appointmentProfileService;
        private readonly IPatientService _patientService;
        private readonly IPatientAccountService _patientAccountService;
        private readonly IServiceClinicDoctorService _serviceClinicDoctorService;
        private readonly IClinicDoctorService _clinicDoctorService;
        private readonly FormCacheHelper _formCacheHelper;

        public AppointmentController(IAppointmentService service,
            IAppointmentProfileService appointmentProfileService,
            IPatientService patientService,
            IPatientAccountService patientAccountService,
            IClinicDoctorService clinicDoctorService,
            IServiceClinicDoctorService serviceClinicDoctorService,
            FormCacheHelper formCacheHelper)
        {
            _service = service;
            _appointmentProfileService = appointmentProfileService;
            _patientService = patientService;
            _patientAccountService = patientAccountService;
            _clinicDoctorService = clinicDoctorService;
            _serviceClinicDoctorService = serviceClinicDoctorService;
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
	        var search = new AppointmentSearch();

	        if (User.IsInRole(UserRoles.Clinic))
		        search.ClinicID = User.ID();
            
	        if (User.IsInRole(UserRoles.Doctor))
		        search.ClinicDoctorID = User.ID();
            
            return await Task.FromResult(View(search));
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(AppointmentSearch search)
        {
	        if (User.IsInRole(UserRoles.Clinic))
		        search.ClinicID = User.ID();
            
	        if (User.IsInRole(UserRoles.Doctor))
		        search.ClinicDoctorID = User.ID();

            return await Task.FromResult(ViewComponent(typeof(AppointmentSearchResultViewComponent), new {search}));
        }

        [HttpGet("[controller]/add")]
        [Authorize(Roles = UserRoles.ClinicOrDoctor)]
        public async Task<IActionResult> Add()
        {
	        return await Save();
        }
        
        [HttpGet("[controller]/edit/{id:int}")]
        [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
        public async Task<IActionResult> Edit(int id)
        {
	        return await Save(id);
        }
        
        [HttpGet("[controller]/modal/{id:int}")]
        [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
        public async Task<IActionResult> Modal(int id)
        {
            return await Save(id, true);
        }

        [HttpPost("[controller]/save")]
        [Authorize(Roles = UserRoles.ClinicOrDoctor)]
        public async Task<IActionResult> Save(Appointment model)
        {
	        if (!ModelState.IsValid)
		        return ValidationProblem();

            model.Patient.Phone = BaseHelpers.CleanPhone(model.Patient.Phone);

	        var response = await RegisterPatient(model);

            if (!response.IsSuccess) 
                return BadRequest(response.Message);

            (var appointment, response) = await SaveAppointment(model);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            if(model.ID <= 0)
                response = await SaveAppointmentProfile(appointment);

            return await ContentResultAsync(response.MessageKey);
        }

        [HttpGet("[controller]/confirm")]
        [Authorize(Roles = UserRoles.ClinicOrDoctor)]
        public async Task<IActionResult> Confirm(string code)
        {
            if (Guid.TryParse(code, out Guid guid))
            {
                var result = await _service.SetArchived(guid);
                var appointmentID = await _service.GetID(guid);

                if (result.IsSuccess)
                    return RedirectToAction("Edit", "Appointment", new { id = appointmentID });
            }

            return RedirectToAction("Index", "Appointment");
        }

        private async Task<CrudResponse> RegisterPatient(Appointment model)
        {
            var accountID = await _patientAccountService.GetAccountID(model.Patient.Phone);

            bool isExists = accountID > 0;

            CrudResponse response;

            model.Patient.PatientAccount = new()
            {
                MobilePhone = model.Patient.Phone,
                Email = model.Patient.Email
            };

            model.Patient.CityID = 1;
            
            if (!isExists)
                response = await _patientService.Register(model.Patient);
            else
            {
                var account = await _patientAccountService.GetAsync(accountID);
                int patientID = account.PatientID > 0 ? account.PatientID : model.Patient.ID;

                model.Patient.ID = patientID;

                response = await _patientService.UpdateAsync(new Patient().InitUpdate(model.Patient));
            }

            return response;
        }

        private async Task<(Appointment appointment, CrudResponse response)> SaveAppointment(Appointment model)
        {
            CrudResponse response;
            Appointment appointment;

            if (model.ID > 0)
            {
                appointment = await _service.GetAsync(model.ID);
                appointment.InitUpdate(model);

                response = await _service.UpdateAsync(appointment);

                string message = "GetWell,\\nСтатус вашей брони поменялся. Проверьте приложение, пожалуйста.";
                await BaseHelpers.SendSms(model.Patient.Phone, message);
            }
            else
            {
                appointment = new Appointment();
                appointment.InitSave(model);
                appointment.ServiceClinicDoctorID = await _serviceClinicDoctorService.GetID(model.ServiceClinicID, model.ClinicDoctorID);
                
                response = await _service.SaveAsync(appointment);

                string message = "GetWell,\\nВаша бронь успешно добавлена.";
                await BaseHelpers.SendSms(model.Patient.Phone, message);
            }

            return (appointment, response);
        }

        private async Task<IActionResult> Save(int id = 0, bool isModal = false)
        {
	        var item = await GetAppointmentAsync(id);
	        
	        item.Patient.Languages = await _formCacheHelper.GetLanguagesAsync(item.Patient.PreferredLanguage);
	        item.Patient.PatientProfile = item.PatientProfile;
	        item.ServiceCategories = await _formCacheHelper.GetServiceCategoriesAsync(item.ServiceCategoryID);
	        item.Services = await _formCacheHelper.GetClinicServicesAsync(item.ServiceClinicID, item.ServiceCategoryID);
			item.Doctors = await _formCacheHelper.GetClinicServiceDoctorsAsync(item.ServiceClinicID, item.ClinicDoctorID); 
            
            if(!isModal) 
	            return View("Save", item);

            return PartialView("SaveModal", item);
        }

        private async Task<Appointment> GetAppointmentAsync(int id = 0)
        {
            var item = id > 0 ? await _service.GetDetailsAsync(id) : new Appointment();

            if (id <= 0)
            {
                item.AppointmentDate = DateTime.UtcNow.AddHours(5);
                item.AppointmentTime = new TimeSpan(item.AppointmentDate.Hour, item.AppointmentDate.Minute, 0);

                item.Patient = new Patient
                {
                    PatientProfile = new PatientProfile()
                    {
                        CityID = 0,
                        RegionID = 0
                    }, 
                    PatientAccount = new PatientAccount()
                };

                item.PatientProfile = item.Patient.PatientProfile;
            }

            if (User.IsInRole(UserRoles.Clinic))
	            item.ClinicID = User.ID();

            if (User.IsInRole(UserRoles.Doctor))
            {
	            item.DoctorID = User.ID();
	            item.ClinicID = await _clinicDoctorService.GetClinicIDAsync(User.ID());
            }
            
            return item;
        }
        
        private async Task<CrudResponse> SaveAppointmentProfile(Appointment appointment)
        {
            return await _appointmentProfileService.SaveAsync(new AppointmentProfile
            {
                AppointmentID = appointment.ID,
                //ClinicDiscountID = model.di,
                QrCodeBase64 = QrCodeHelper.GenerateQrCodeImage(appointment.ConfirmationCode.ToString()),
                IsResolved = false
            });
        }
    }
}
