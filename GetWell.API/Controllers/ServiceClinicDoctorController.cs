using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.API.Controllers
{
    public class ServiceClinicDoctorController : BaseApiController<ServiceClinicDoctor>
    {
        #region ctor

        private readonly IServiceClinicDoctorService _service;

        public ServiceClinicDoctorController(IServiceClinicDoctorService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
        {
            _service = service;
        }

        #endregion
    }
}