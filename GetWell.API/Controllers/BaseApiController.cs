using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public abstract class BaseApiController<T> : ControllerBase, IBaseApiController<T> where T : class, IBaseModel, new() 
	{
        #region ctor

        private readonly IBaseService<T> _service;
        private readonly PatientCacheHelper _patientCacheHelper;

        protected BaseApiController(IBaseService<T> service, PatientCacheHelper patientCacheHelper)
        {
            _service = service;
            _patientCacheHelper = patientCacheHelper;
        }

        #endregion


		[HttpGet("get/{id:int}")]
		public virtual async Task<ActionResult<T>> Get(int id)
		{
			var result = await _service.GetAsync(id);

			InitLocalization(result);

			return Ok(result);
		}
		
		[HttpGet("getall")]
		public virtual async Task<ActionResult<PaginatedList<T>>> GetAll(int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
		{
			var result = await _service.GetAllAsync(pageNumber, pageSize, orderBy, isAsc);

			InitLocalization(result);

			return Ok(result);
		}

		[Authorize]
		[HttpPost("add")]
		public virtual async Task<ActionResult<CrudResponse>> Add(T model)
		{
			var result = await _service.SaveAsync(model);

			return Ok(result);
		}

		[Authorize]
		[HttpPut("save/{id:int}")]
		public virtual async Task<ActionResult<CrudResponse>> Save(int id, T model)
		{
			//basic validation
			if (id != model.ID)
				return BadRequest();
			
			var result = await _service.UpdateAsync(model);

			return Ok(result);
		}

		[Authorize]
		[HttpDelete("delete/{id:int}")]
		public virtual async Task<ActionResult<CrudResponse>> Delete(int id)
		{
			var result = await _service.DeleteAsync(id);

			return Ok(result);
		}
		
		
		protected async Task<CrudResponse> Result(Crud crud)
		{
			return await Task.FromResult(new CrudResponse(crud));
		}
		
		protected void InitLocalization(T result)
		{
            if (result is IBaseLocalizable<T> localizable)
            {
                localizable.Init(PatientLanguage);
            }
        }

		protected void InitLocalization(List<T> result)
		{
			result.ForEach(InitLocalization);
		}

        protected LocalizationLanguageEnum PatientLanguage
        {
            get
            {
                var language = LocalizationLanguageEnum.Ru;

                var task = Task.Run(async () => { language = await _patientCacheHelper.GetLanguage(PatientID); });

                task.Wait();

                return language;
            }
        }
        
        protected int PatientCityID
        {
            get
            {
                int cityID = 0;

                var task = Task.Run(async () => { cityID = await _patientCacheHelper.GetCityID(PatientID); });

                task.Wait();

                return cityID;
            }
        }

        protected Patient PatientProfile
        {
            get
            {
                var patient = new Patient();

                var task = Task.Run(async () => { patient = await _patientCacheHelper.GetProfile(PatientID); });

                task.Wait();

                return patient;
            }
        }

        protected int PatientID => User.Identity is { IsAuthenticated: true } && User.IsInRole(UserRoles.Patient) ? User.ID() : 0;
    }
}