using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class CategoryController : BaseApiController<Category>
	{
		#region ctor

		private readonly ICategoryService _service;

		public CategoryController(ICategoryService service, 
            PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [HttpGet("getall")]
        public override async Task<ActionResult<PaginatedList<Category>>> GetAll(int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
        {
            return await base.GetAll(pageSize, pageNumber, "SortOrder", isAsc);
        }

        [AllowAnonymous]
		[HttpGet("get-all")]
		public async Task<ActionResult<List<Category>>> GetAllSorted()
		{
			var result = await _service.GetAllAsync();

			InitLocalization(result);

			return Ok(result);
		}
	}
}