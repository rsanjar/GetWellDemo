using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Core.Mailer;
using GetWell.Dashboard.Models;
using GetWell.Dashboard.UserServices;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetWell.Dashboard.Controllers
{
	public class AccountController : Controller
	{
		#region ctor

		private readonly ILogger<AccountController> _logger;
        private readonly IClinicAccountService _clinicAccountService;
        private readonly IClinicDoctorAccountService _clinicDoctorAccountService;
        private IAuthenticatable _service;
		private readonly UserServiceFactory _userServiceFactory;
        private readonly IEmailSender _emailSender;
		
		public AccountController(
			ILogger<AccountController> logger, 
			IClinicAccountService clinicAccountService,
			IClinicDoctorAccountService clinicDoctorAccountService,
			UserServiceFactory userServiceFactory, IEmailSender emailSender)
		{
			_logger = logger;
            _clinicAccountService = clinicAccountService;
            _clinicDoctorAccountService = clinicDoctorAccountService;
            _userServiceFactory = userServiceFactory;
            _emailSender = emailSender;
        }

		#endregion

		[AllowAnonymous]
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpGet]
		public ActionResult ForgotPassword()
		{
			return View();
		}

        [Authorize]
        [HttpGet("[controller]/change-password")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost("[controller]/change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return new ContentResult() { Content = "Failure" };

            _service = _userServiceFactory.Create(User.Role());
            var result = await _service.UpdatePassword(User.Identity.Name, request.Password, request.NewPassword);
            
            if (result)
            {
                return new ContentResult(){ Content = "Success" };
            }
            
            return new ContentResult() { Content = "Failure" };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            request.UserName = BaseHelpers.CleanPhone(request.UserName);
            request.Role = request.Role.TrimCapitalize(UserRoles.Doctor); 

			if(request.Role != UserRoles.Doctor && 
               request.Role != UserRoles.Clinic && 
               request.Role != UserRoles.Admin)
				return new ContentResult(){ Content = "Failure" };

            _service = _userServiceFactory.Create(request.Role);
            _logger.LogInformation($"Reminding User Password for: [{request.UserName}]");

            var result = await _service.GetPassword(request.UserName);

            if (!string.IsNullOrWhiteSpace(result))
            {
                string message = "GetWell,\\nВы попросили напомнить ваш пароль: \\n" + result;
                await BaseHelpers.SendSms(request.UserName, message);

                var email = await _service.GetEmail(request.UserName);

                if (!string.IsNullOrWhiteSpace(email))
                {
                    message = $"<h4>Getwell,</h4><p>Вы попросили напомнить ваш пароль: {result}</p><br/><p>Администрация GetWell</p>";

                    await _emailSender.SendEmailAsync(email, "GetWell - Ваш Пароль", message);
                }

                return new ContentResult(){ Content = "Success" };
            }

            return new ContentResult(){ Content = "Failure" };
        }

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult> Login([FromForm] LoginRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest();
		    
			if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
				return Unauthorized(request);

            request.UserName = BaseHelpers.CleanPhone(request.UserName);
			request.Role = request.Role.TrimCapitalize(UserRoles.Patient); 

			_service = _userServiceFactory.Create(request.Role);
			_logger.LogInformation($"Validating user [{request.UserName}]");
			
			var isValid = await _service.IsValidUserCredentialsAsync(request.UserName, request.Password);

			if (!isValid)
				return Unauthorized(request);

			//accountID is used to grab doctor, clinic, patient or admin personal details.
			request.AccountID = await _service.GetAccountID(request.UserName, request.Password);
			await _service.UpdateLastLoginDate(request.UserName);
			
			return await GenerateAuthentication(request);
		}
		
		[HttpGet]
		[Authorize]
		public async Task<ActionResult> SignOutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return Redirect(Url.Action("Index", "Home"));
		}
		
		private async Task<ActionResult> GenerateAuthentication(LoginRequest request)
		{
			var claims = new List<Claim>
			{
				new (ClaimTypes.Name,request.UserName),
				new (ClaimTypes.Role, request.Role),
				new (ClaimTypes.Sid, request.AccountID.ToString())
			};

            if (request.Role == UserRoles.Clinic)
            {
                var account = await _clinicAccountService.GetAsync(request.AccountID);
				
                claims.Add(new Claim(ClaimTypes.PrimarySid, account.ClinicID.ToString()));
            }

            if (request.Role == UserRoles.Doctor)
            {
                var account = await _clinicDoctorAccountService.GetAsync(request.AccountID);

                claims.Add(new Claim(ClaimTypes.PrimarySid, account.ClinicDoctorID.ToString()));
            }
			
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("Index", "Home"),
				AllowRefresh = true,
				IsPersistent = true,
				IssuedUtc = DateTime.UtcNow
			};

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity), authProperties);

			return Json(new { redirectToUrl = Url.Action("Index", "Home") });
		}
	}
}