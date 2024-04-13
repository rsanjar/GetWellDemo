using System.Net;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class NewsController : BaseApiController<News>
	{
		#region ctor

		private readonly INewsService _service;

		public NewsController(INewsService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion

        [AllowAnonymous]
        [HttpGet("getall")]
        public override async Task<ActionResult<PaginatedList<News>>> GetAll(int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize, orderBy, isAsc) ?? new PaginatedList<News>();
            
            InitLocalization(result);

            result.ForEach(c => c.Body = WebUtility.HtmlDecode(c.Body));

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("get/{id:int}")]
        public override async Task<ActionResult<News>> Get(int id)
        {
            var result = await _service.GetAsync(id) ?? new News();

            InitLocalization(result);

            result.Body = WebUtility.HtmlDecode(result.Body);

            return Ok(result);
        }
    }
}