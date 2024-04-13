using System;
using System.Linq;
using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class AppointmentController :  BaseApiController<Appointment>
	{
		#region ctor

		private readonly IAppointmentService _service;
		private readonly IAppointmentProfileService _appointmentProfileService;
		private readonly IClinicDiscountService _clinicDiscountService;
		private readonly IServiceClinicService _serviceClinicService;
        private readonly IClinicPhoneService _clinicPhoneService;
        private readonly IDoctorPhoneService _doctorPhoneService;

        public AppointmentController(IAppointmentService service,
            IAppointmentProfileService appointmentProfileService,
            IClinicDiscountService clinicDiscountService,
            IServiceClinicService serviceClinicService,
            IClinicPhoneService clinicPhoneService,
            IDoctorPhoneService doctorPhoneService, 
            PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
			_appointmentProfileService = appointmentProfileService;
			_clinicDiscountService = clinicDiscountService;
			_serviceClinicService = serviceClinicService;
			_clinicPhoneService = clinicPhoneService;
            _doctorPhoneService = doctorPhoneService;
        }

		#endregion

		[Authorize]
		[HttpPost("search")]
		public async Task<ActionResult<PaginatedList<Appointment>>> GetAll(AppointmentSearch search)
		{
			var result = await _service.GetAllAsync(search);
			
			return Ok(result);
		}

		[Authorize(Roles = UserRoles.Patient)]
		[HttpPost("patient-appointments")]
		public async Task<ActionResult<PaginatedList<Appointment>>> GetAll(PaginationModel<Appointment> pagination)
		{
			var result = await _service.GetAllAsync(User.ID(), pagination);

			InitLocalization(result);

			result.ForEach(c => c.Doctor.Init(PatientLanguage));
			result.ForEach(c => c.Clinic.Init(PatientLanguage));
			
			return Ok(result);
		}

		[Authorize]
		[HttpGet("get/{id:int}")]
		public override async Task<ActionResult<Appointment>> Get(int id)
		{
			var result = await _service.GetDetailsAsync(id);

			return Ok(result);
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[AllowAnonymous]
        [HttpPost("not-implemented")]
		public override Task<ActionResult<CrudResponse>> Add(Appointment model)
		{
			throw new NotImplementedException();
		}

		[Authorize(Roles = UserRoles.Patient)]
		[HttpPost("create")]
		public async Task<ActionResult> Add(AppointmentRequest model)
		{
            var appointment = (Appointment)model;
			
            appointment.PatientID = User.ID();

            if (!(await IsAppointmentDateValid(model)))
                return BadRequest("Invalid appointment date");

			await SetServicePriceAndDuration(model.ServiceClinicDoctorID, appointment);
			await SetDiscountPercentage(model.ClinicDiscountID.GetValueOrDefault(0), appointment);
			
			var result = await _service.SaveAsync(appointment);

			if (!result.IsSuccess)
				return BadRequest(result.Message);

			result = await SaveAppointmentProfile(model.ClinicDiscountID.GetValueOrDefault(0), appointment);

			if (!result.IsSuccess)
				return BadRequest(result.Message);

            await SendSms(appointment);

			return Ok(appointment.ID);
		}
		
        [Authorize(Roles = UserRoles.Patient)]
		[HttpPost("add-review")]
		public async Task<ActionResult> AddReview(int appointmentID, int rating, string review)
		{
			var result = await _service.AddReviewAsync(new Appointment()
			{
				ID = appointmentID,
				PatientID = User.ID(),
				AppointmentProfile = new AppointmentProfile()
				{
					PatientRating = rating,
					PatientReview = review
				}
			});

			return Ok(result);
		}

        [Authorize(Roles = UserRoles.Patient)]
		[HttpGet("get-qr-code-base64/{appointmentID:int}")]
        public async Task<ActionResult> GetQrBase64Image(int appointmentID)
        {
            var result = await _service.GetQrImageBase64Async(appointmentID, User.ID());

            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Patient)]
        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel(int appointmentID)
        {
            var result = await _service.CancelAppointmentAsync(appointmentID, User.ID());

            return Ok(result);
        }

        [AllowAnonymous]
		[HttpGet("get-new-count-by-clinic")]
        public async Task<ActionResult> GetCountByClinic(int clinicID)
        {
            int count = await _service.GetCountByClinic(clinicID);

            return Ok(count);
        }

        [AllowAnonymous]
        [HttpGet("get-new-count-by-doctor")]
        public async Task<ActionResult> GetCountByClinicDoctor(int clinicDoctorID)
        {
            int count = await _service.GetCountByClinicDoctor(clinicDoctorID);

            return Ok(count);
        }


        #region Private Methods

		private async Task<bool> IsAppointmentDateValid(AppointmentRequest model)
        {
            var appointmentDate = new DateTime(model.AppointmentYear, model.AppointmentMonth, 
                model.AppointmentDay, model.AppointmentHour, model.AppointmentMinute, 0);
			
            if (model.ClinicDiscountID > 0)
            {
                var discount = await _clinicDiscountService.GetAsync(model.ClinicDiscountID ?? 0);
			
                return appointmentDate >= discount.StartDate && appointmentDate <= discount.EndDate;
            }

            return appointmentDate > DateTime.UtcNow.AddHours(-1);
        }

		private async Task SetDiscountPercentage(int clinicDiscountID, Appointment appointment)
		{
			if (clinicDiscountID > 0)
			{
				var discount = await _clinicDiscountService.GetAsync(clinicDiscountID);

				if (discount != null)
					appointment.DiscountPercentage = (int)discount.DiscountPercentage;
			}
		}

		private async Task SetServicePriceAndDuration(int serviceClinicDoctorID, Appointment appointment)
		{
			var serviceClinic = await _serviceClinicService.GetByClinicDoctorAsync(serviceClinicDoctorID);

			appointment.PriceBeforeDiscount = serviceClinic.Price;
			appointment.AverageDuration = new TimeSpan(0, serviceClinic.Duration.GetValueOrDefault(0), 0);
		}

		private async Task<CrudResponse> SaveAppointmentProfile(int clinicDiscountID, Appointment appointment)
		{
			return await _appointmentProfileService.SaveAsync(new AppointmentProfile
			{
				AppointmentID = appointment.ID,
				ClinicDiscountID = clinicDiscountID,
				QrCodeBase64 = QrCodeHelper.GenerateQrCodeImage($"https://dashboard.getwell.uz/appointment?code={appointment.ConfirmationCode}"),
				IsResolved = false
			});
		}

        private async Task SendSms(Appointment appointment)
        {
            if (appointment.ID > 0)
            {
                try
                {
                    await SmsDoctor(appointment.ServiceClinicDoctorID);
                    await SmsClinic(appointment.ServiceClinicDoctorID);
                }
                catch
                {
                    ; //send email
                }
            }
        }

        private async Task SmsDoctor(int serviceClinicDoctorID)
        {
            var phones = await _clinicPhoneService.GetAllByServiceClinicDoctorAsync(serviceClinicDoctorID);

            var phone = phones.Any() ? phones[0].Phone : string.Empty;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                string message = "Get Well,\\nУ вас новая бронь, подтвердите в админной панели, пожалуйста";

                await BaseHelpers.SendSms(phone, message);
            }
        }

        private async Task SmsClinic(int serviceClinicDoctorID)
        {
            var phones = await _doctorPhoneService.GetAllByServiceClinicDoctorAsync(serviceClinicDoctorID);

            var phone = phones.Any() ? phones[0].Phone : string.Empty;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                string message = "Get Well,\\nУ вас новая бронь, подтвердите в админной панели, пожалуйста";

                await BaseHelpers.SendSms(phone, message);
            }
        }

		#endregion
	}
}