using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers;

[Authorize(Roles = UserRoles.Patient)]
public class PatientFavoriteDoctorController : BaseApiController<PatientFavoriteDoctor>
{
	#region ctor

	private readonly IPatientFavoriteDoctorService _service;

	public PatientFavoriteDoctorController(IPatientFavoriteDoctorService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
	{
		_service = service;
	}

	#endregion

    [ApiExplorerSettings(IgnoreApi = true)]
	[HttpPost("not-implemented")]
    public override Task<ActionResult<CrudResponse>> Add(PatientFavoriteDoctor model)
    {
        throw new NotImplementedException("not implemented");
    }

    [Authorize(Roles = UserRoles.Patient)]
    [HttpPost("add")]
    public async Task<ActionResult<CrudResponse>> AddItem(int clinicDoctorID)
    {
        var result = await _service.AddAsync(clinicDoctorID, User.ID());

        return Ok(result);
    }

	[Authorize(Roles = UserRoles.Patient)]
	[HttpGet("get-all")]
	public async Task<ActionResult<List<PatientFavoriteDoctor>>> GetAll()
	{
		var list = await _service.GetAllAsync(User.ID());
		
        InitLocalization(list);

        list.ForEach(c => c.Doctor.Init(PatientLanguage));
        list.ForEach(c => c.DoctorProfile.Init(PatientLanguage));

		return Ok(list);
	}

	[Authorize(Roles = UserRoles.Patient)]
	[HttpPost("remove")]
	public async Task<ActionResult<CrudResponse>> Remove(int clinicDoctorID)
	{
		var list = await _service.RemoveAsync(clinicDoctorID, User.ID());
		
		return Ok(list);
	}
}