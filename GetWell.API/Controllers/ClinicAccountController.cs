using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicAccountController : BaseApiController<ClinicAccount>
	{
		#region ctor

		private readonly IClinicAccountService _service;

		public ClinicAccountController(IClinicAccountService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

		[Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get/{id:int}")]
        public override async Task<ActionResult<ClinicAccount>> Get(int id)
		{
			return await base.Get(id);
		}

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getall/{clinicID:int}")]
		public async Task<ActionResult<ClinicAccount>> GetByClinic(int clinicID)
		{
			return await _service.GetByClinicAsync(clinicID);
		}

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("getall")]
		public override async Task<ActionResult<PaginatedList<ClinicAccount>>> GetAll(int pageSize = 5, 
			int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
		{
			return await base.GetAll(pageSize, pageNumber, orderBy, isAsc);
		}

		[Authorize(Roles = UserRoles.Admin)]
        [HttpPost("add")]
		public override async Task<ActionResult<CrudResponse>> Add(ClinicAccount model)
		{
			return await base.Add(model);
		}

		[Authorize(Roles = UserRoles.Admin)]
        [HttpPut("save/{id:int}")]
		public override async Task<ActionResult<CrudResponse>> Save(int id, ClinicAccount model)
		{
			return await base.Save(id, model);
		}

		[Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete/{id:int}")]
		public override async Task<ActionResult<CrudResponse>> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}