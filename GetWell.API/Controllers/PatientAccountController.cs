using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
	public class PatientAccountController : BaseApiController<PatientAccount>
	{
		#region ctor

		private readonly IPatientAccountService _service;

		public PatientAccountController(IPatientAccountService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

		[Authorize(Roles = UserRoles.Admin)]
		public override async Task<ActionResult<PatientAccount>> Get(int id)
		{
			return await base.Get(id);
		}

		[Authorize(Roles = UserRoles.Admin)]
		public override async Task<ActionResult<PaginatedList<PatientAccount>>> GetAll(int pageSize = 5, 
			int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
		{
			return await base.GetAll(pageSize, pageNumber, orderBy, isAsc);
		}

		[Authorize(Roles = UserRoles.Admin)]
		public override async Task<ActionResult<CrudResponse>> Add(PatientAccount model)
		{
			return await base.Add(model);
		}

		[Authorize(Roles = UserRoles.Admin)]
		public override async Task<ActionResult<CrudResponse>> Save(int id, PatientAccount model)
		{
			return await base.Save(id, model);
		}

		[Authorize(Roles = UserRoles.Admin)]
		public override async Task<ActionResult<CrudResponse>> Delete(int id)
		{
			return await base.Delete(id);
		}
	}
}