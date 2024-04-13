using GetWell.Core.Helpers;
using GetWell.Core;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GetWell.API.Controllers;

[Authorize(Roles = UserRoles.Patient)]
public class PatientFavoriteClinicController : BaseApiController<PatientFavoriteClinic>
{

	#region ctor

	private readonly IPatientFavoriteClinicService _service;

	public PatientFavoriteClinicController(IPatientFavoriteClinicService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
	{
		_service = service;
	}

	#endregion

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("not-implemented")]
    public override Task<ActionResult<CrudResponse>> Add(PatientFavoriteClinic model)
    {
        throw new NotImplementedException("not implemented");
    }

    [Authorize(Roles = UserRoles.Patient)]
    [HttpPost("add")]
    public async Task<ActionResult<CrudResponse>> AddItem(int clinicID)
    {
        var result = await _service.AddAsync(clinicID, User.ID());

        return Ok(result);
    }

	[Authorize(Roles = UserRoles.Patient)]
	[HttpGet("get-all")]
	public async Task<ActionResult<List<PatientFavoriteClinic>>> GetAll()
	{
		var list = await _service.GetAllAsync(User.ID());

		InitLocalization(list);

        list.ForEach(c => c.Clinic.Init(PatientLanguage));

		return Ok(list);
	}

	[Authorize(Roles = UserRoles.Patient)]
	[HttpPost("remove")]
	public async Task<ActionResult<CrudResponse>> Remove(int clinicID)
	{
		var list = await _service.RemoveAsync(clinicID, User.ID());

		return Ok(list);
	}
}