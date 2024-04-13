using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GetWell.API.JwtManager;
using GetWell.API.Models;
using GetWell.API.UserServices;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.DTO;
using GetWell.DTO.Localization;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GetWell.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class AccountController : ControllerBase
    {
	    #region ctor

	    private readonly ILogger<AccountController> _logger;
	    private readonly IJwtAuthManager _jwtAuthManager;
	    private IAuthenticatable _service;
	    private readonly UserServiceFactory _userServiceFactory;
	    private readonly IPatientService _patientService;
		private readonly IClinicAccountService _clinicAccountService;
		private readonly IClinicDoctorAccountService _clinicDoctorAccountService;
		private readonly IPatientAccountService _patientAccountService;
        private readonly PatientCacheHelper _patientCacheHelper;
        private readonly ConfigHelper _configHelper;

	    public AccountController(
		    ILogger<AccountController> logger, 
		    IJwtAuthManager jwtAuthManager,
		    UserServiceFactory userServiceFactory,
		    IPatientService patientService,
		    ConfigHelper configHelper, 
		    IClinicAccountService clinicAccountService,
		    IClinicDoctorAccountService clinicDoctorAccountService,
		    IPatientAccountService patientAccountService,
            PatientCacheHelper patientCacheHelper)
	    {
		    _logger = logger;
		    _jwtAuthManager = jwtAuthManager;
			_userServiceFactory = userServiceFactory;
			_patientService = patientService;
			_configHelper = configHelper;
			_patientAccountService = patientAccountService;
            _patientCacheHelper = patientCacheHelper;
            _clinicAccountService = clinicAccountService;
			_clinicDoctorAccountService = clinicDoctorAccountService;
	    }

	    #endregion

		[AllowAnonymous]
	    [HttpPost("login")]
	    public async Task<ActionResult> Login([FromBody] LoginRequest request)
	    {
		    if (!ModelState.IsValid)
			    return BadRequest();
		    
		    if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
			    return Unauthorized();
		    
		    request.Role = request.Role.TrimCapitalize(UserRoles.Patient); 

		    _service = _userServiceFactory.Create(request.Role);
			_logger.LogInformation($"Validating user [{request.UserName}]");
			
		    var isValid = await _service.IsValidUserCredentialsAsync(request.UserName, request.Password);
		    
		    if (!isValid)
			    return Unauthorized();

		    request.AccountID = await _service.GetAccountID(request.UserName, request.Password);
		    await _service.UpdateLastLoginDate(request.UserName);
			
		    return await GenerateAuthentication(request);
	    }

		[AllowAnonymous]
		[HttpPost("send-phone-verification")]
	    public async Task<ActionResult> SendPhoneVerificationCode(
		    [FromHeader(Name = "secret-key")]string secretKey, 
		    [FromForm]string phone)
	    {
		    if (string.IsNullOrWhiteSpace(secretKey) || secretKey != _configHelper.SmsConfig.SecretKey)
			    return Unauthorized();

            bool isNewAccount = false;

		    _service = _userServiceFactory.Create();

            phone = BaseHelpers.CleanPhone(phone);

            int code = await _service.GetSmsActivationCode(phone);

			if(code <= 0) //if no user found then register
            {
                var result = await RegisterTempUser(phone);

                if (result.IsSuccess)
                    code = await _service.GetSmsActivationCode(phone);
                
                isNewAccount = true;
            }

            if (code <= 0)
                return BadRequest("Invalid Phone");

			string message = "Get Well,\\nВаш код для авторизации\\nAvtorizatsiya uchun kodingiz:\\n" + code;
			
            if (!(await BaseHelpers.SendSms(phone, message)))
            {
                return BadRequest("Error sending an SMS");
            }

		    return Ok(new { isNew = isNewAccount });
	    }
		
        [AllowAnonymous]
	    [HttpPost("patient-login")]
	    public async Task<ActionResult> PhoneLogin([FromBody] PhoneLoginRequest request)
	    {
		    if (!ModelState.IsValid)
			    return BadRequest();
		    
		    if (string.IsNullOrWhiteSpace(request.Phone) || request.SmsActivationCode < 0)
			    return Unauthorized();
		    
            request.Phone = BaseHelpers.CleanPhone(request.Phone);

		    _service = _userServiceFactory.Create();
		    _logger.LogInformation($"Validating user [{request.Phone}]");
			
		    var isValid = await ValidatePhone(request);

			if(!isValid)
				return Unauthorized();
			
			await _service.UpdateLastLoginDate(request.Phone);
			
            return await GenerateAuthentication(new LoginRequest
            {
				AccountID = request.AccountID,
				Role = UserRoles.Patient,
				UserName = request.Phone
            });
	    }
		
	    [Authorize(Roles = UserRoles.Patient)]
	    [HttpPost("save-profile")]
        public async Task<ActionResult> SavePatientProfile([FromBody] PatientProfileRequest request)
	    {
		    if (!ModelState.IsValid)
			    return BadRequest();

			if(request.CityID <= 0)
                request.CityID = 1;

            var result = await _patientService.SaveProfile(new Patient()
            {
                ID = User.ID(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                CityID = request.CityID,
                PreferredLanguage = (int)request.Language,
                PatientProfile = new PatientProfile()
                {
                    DateOfBirth = request.DateOfBirth,
                    IsMale = request.IsMale
                }
            });

            if (result.IsSuccess)
            {
				_patientCacheHelper.Reset(User.ID());
				
                return Ok();
            }
			  
		    return ValidationProblem(result.Message);
	    }

        [Authorize(Roles = UserRoles.Patient)]
        [HttpPost("disable")]
        public async Task<ActionResult> DisableProfile()
        {
            await _patientAccountService.Disable(User.ID());

            return Logout();
        }

        [Authorize(Roles = UserRoles.Patient)]
        [HttpGet("get-profile")]
        public async Task<ActionResult> GetPatientProfile()
        {
            var profile = await _patientService.GetProfile(User.ID());
			
			profile.Init((LocalizationLanguageEnum)profile.PreferredLanguage);

            var result = new PatientProfileRequest()
            {
				FirstName = profile.FirstName,
				LastName = profile.LastName,
				PreferredLanguage = ((LanguageEnum)profile.PreferredLanguage).ToString(),
				CityID = profile.CityID ?? 0,
                CityName = profile.CityName,
				DateOfBirth = profile.PatientProfile.DateOfBirth,
				IsMale = profile.PatientProfile.IsMale
            };

			return Ok(result);
        }

	    [HttpGet("user")]
	    [Authorize]
	    public ActionResult GetCurrentUser()
	    {
		    if (User.Identity == null) 
			    return Unauthorized();

		    return Ok(new LoginResult
		    {
			    UserName = User.Identity.Name,
			    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
			    OriginalUserName = User.FindFirst("OriginalUserName")?.Value
		    });

	    }

	    [HttpPost("logout")]
	    [Authorize]
	    public ActionResult Logout()
	    {
		    // optionally "revoke" JWT token on the server side --> add the current token to a block-list
		    // https://github.com/auth0/node-jsonwebtoken/issues/375

		    if (User.Identity == null) 
			    return Ok();

		    var userName = User.Identity.Name;
		    _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
		    _logger.LogInformation($"User [{userName}] logged out the system.");

		    return Ok();

	    }

	    [HttpPost("refresh-token")]
	    [Authorize]
	    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
	    {
		    try
		    {
			    if (User.Identity == null) 
				    return await Task.FromResult<ActionResult>(Ok());

			    var userName = User.Identity.Name;
			    _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

			    if (string.IsNullOrWhiteSpace(request.RefreshToken))
			    {
				    return Unauthorized();
			    }

			    var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
			    var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
			    _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
			    return Ok(new LoginResult
			    {
				    UserName = userName,
				    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
				    AccessToken = jwtResult.AccessToken,
				    RefreshToken = jwtResult.RefreshToken.TokenString
			    });
		    }
		    catch (SecurityTokenException e)
		    {
			    return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
		    }
	    }

	    [HttpPost("impersonation")]
	    [Authorize(Roles = UserRoles.Admin)]
	    public async Task<ActionResult> Impersonate([FromBody] ImpersonationRequest request)
	    {
		    if (User.Identity == null) 
			    return await Task.FromResult<ActionResult>(Ok());

		    var userName = User.Identity.Name;
		    _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

		    var impersonatedRole = UserRoles.Admin; //await _userService.GetUserRole(request.UserName);
		    if (string.IsNullOrWhiteSpace(impersonatedRole))
		    {
			    _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
			    return BadRequest($"The target user [{request.UserName}] is not found.");
		    }
		    if (impersonatedRole == UserRoles.Admin)
		    {
			    _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
			    return BadRequest("This action is not supported.");
		    }

		    var claims = new[]
		    {
			    new Claim(ClaimTypes.Name,request.UserName),
			    new Claim(ClaimTypes.Role, impersonatedRole),
			    new Claim("OriginalUserName", userName ?? string.Empty)
		    };

		    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
		    _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
		    return Ok(new LoginResult
		    {
			    UserName = request.UserName,
			    Role = impersonatedRole,
			    OriginalUserName = userName,
			    AccessToken = jwtResult.AccessToken,
			    RefreshToken = jwtResult.RefreshToken.TokenString
		    });
	    }

	    [HttpPost("stop-impersonation")]
	    public async Task<ActionResult> StopImpersonation()
	    {
		    if (User.Identity == null) 
			    return await Task.FromResult<ActionResult>(Ok());

		    var userName = User.Identity.Name;
		    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
		    if (string.IsNullOrWhiteSpace(originalUserName))
		    {
			    return BadRequest("You are not impersonating anyone.");
		    }
		    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

		    var role = UserRoles.Admin; //await _userService.GetUserRole(originalUserName);
		    var claims = new[]
		    {
			    new Claim(ClaimTypes.Name,originalUserName),
			    new Claim(ClaimTypes.Role, role)
		    };

		    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
		    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
		    return Ok(new LoginResult
		    {
			    UserName = originalUserName,
			    Role = role,
			    OriginalUserName = null,
			    AccessToken = jwtResult.AccessToken,
			    RefreshToken = jwtResult.RefreshToken.TokenString
		    });

	    }
		
	    private async Task<ActionResult> GenerateAuthentication(LoginRequest request)
	    {
		    var claims = new List<Claim>
		    {
			    new (ClaimTypes.Name,request.UserName),
			    new (ClaimTypes.Role, request.Role)
		    };
			
		    switch (request.Role)
            {
                case UserRoles.Patient:
                {
                    var patientAccount = await _patientAccountService.GetAsync(request.AccountID);

                    claims.Add(new Claim(ClaimTypes.PrimarySid, patientAccount.PatientID.ToString()));
                    claims.Add(new Claim(ClaimTypes.Sid, patientAccount.ID.ToString()));
                    break;
                }
                case UserRoles.Clinic:
                {
                    var account = await _clinicAccountService.GetAsync(request.AccountID);
				
                    claims.Add(new Claim(ClaimTypes.PrimarySid, account.ClinicID.ToString()));
                    break;
                }
                case UserRoles.Doctor:
                {
                    var account = await _clinicDoctorAccountService.GetAsync(request.AccountID);

                    claims.Add(new Claim(ClaimTypes.PrimarySid, account.ClinicDoctorID.ToString()));
                    break;
                }
                default:
                    claims.Add(new Claim(ClaimTypes.PrimarySid, request.AccountID.ToString()));
                    break;
            }

		    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims.ToArray(), DateTime.Now);
		    _logger.LogInformation($"User [{request.UserName}] logged in the system.");
		    return Ok(new LoginResult
		    {
			    UserName = request.UserName,
			    Role = request.Role,
			    AccessToken = jwtResult.AccessToken,
			    RefreshToken = jwtResult.RefreshToken.TokenString
		    });
	    }
		
	    private async Task<bool> ValidatePhone(PhoneLoginRequest request)
	    {
            bool isValid = await _service.IsValidUserCredentialsAsync(request.Phone, request.SmsActivationCode);
		
		    if(!isValid)
			    _logger.LogInformation($"Phone validation failed: [{request.Phone}]");
			
		    request.AccountID = await _service.GetAccountID(request.Phone);

            // if (request.Phone == "998901234567")
            // {
            //     isValid = true;
            //     request.AccountID = 1;
            // }

		    return isValid;
	    }
		
        private async Task<CrudResponse> RegisterTempUser(string phone)
        {
            RegisterPatientRequest registerPatientRequest = new()
            {
                FirstName = "Temp",
                LastName = "Temp",
                Phone = phone,
                PreferredLanguage = (int)LanguageEnum.Ru
            };

            var patient = (Patient)registerPatientRequest;

            patient.Email = "noemail@temp.com";
            patient.IsActive = true;
            patient.CityID = 1;

            var result = await _patientService.Register(patient);

            return result;
        }
    }
}
