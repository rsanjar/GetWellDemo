using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers;

public class ClinicDiscountController :  BaseApiController<ClinicDiscount>
{
    #region ctor

    private readonly IClinicDiscountService _service;

    public ClinicDiscountController(IClinicDiscountService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
    {
        _service = service;
    }

    #endregion

	[AllowAnonymous]
	[HttpGet("get-all-active")]
	public async Task<ActionResult<List<ClinicDiscount>>> GetAllActive()
	{
		var list = await _service.GetAllActive();

        InitLocalization(list);

        list.ForEach(c => c.Body = WebUtility.HtmlDecode(c.Body));
		
		return Ok(list);
	}
}