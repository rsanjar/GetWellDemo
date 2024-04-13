using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO.Interfaces;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public abstract class BaseApiReadOnlyController<T> : ControllerBase, IBaseApiReadController<T> where T : class, IBaseModel, new() 
	{
        #region ctor

        private readonly IBaseService<T> _service;
		
        protected BaseApiReadOnlyController(IBaseService<T> service)
        {
            _service = service;
        }

        #endregion


		[HttpGet("get/{id:int}")]
		public virtual async Task<ActionResult<T>> Get(int id)
		{
			var result = await _service.GetAsync(id);

			return Ok(result);
		}

		[HttpGet("getall")]
		public virtual async Task<ActionResult<PaginatedList<T>>> GetAll(int pageSize = 5, 
			int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
		{
			var result = await _service.GetAllAsync(pageNumber, pageSize, orderBy, isAsc);
			
			return Ok(result);
		}
    }
}