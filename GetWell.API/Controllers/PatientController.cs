using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class PatientController : BaseApiController<Patient>
	{
		#region ctor

		private readonly IPatientService _service;
        private readonly IPatientProfileService _patientProfileService;

		public PatientController(IPatientService service, 
            IPatientProfileService patientProfileService, 
            PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
        {
            _service = service;
            _patientProfileService = patientProfileService;
        }

		#endregion

        [Authorize(Roles = UserRoles.Patient)]
        [HttpGet("get-profile-picture")]
        public async Task<ActionResult> GetProfilePicture()
        {
            var result = await _patientProfileService.GetProfilePhotoBase64(User.ID());

            return Ok(new { base64Image = result });
        }

        [Authorize(Roles = UserRoles.Patient)]
        [HttpPost("add-profile-picture")]
        public async Task<ActionResult> AddProfilePicture([FromBody]string base64Image)
        {
            base64Image = BaseHelpers.ResizeBase64(base64Image);

            if(base64Image.Length > 50000)
                return ValidationProblem("Image file is too large");

            var result = await _patientProfileService.UpdateProfilePhoto(User.ID(), base64Image);

            return Ok(result);
        }
	}
}