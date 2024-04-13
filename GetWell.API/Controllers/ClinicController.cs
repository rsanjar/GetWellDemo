// Ignore Spelling: API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.Core.Helpers;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicController : BaseApiController<Clinic>
	{
		#region ctor

		private readonly IClinicService _service;
		
		public ClinicController(IClinicService service, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
		}

		#endregion
        
        [AllowAnonymous]
        [HttpGet("getall")]
		public override async Task<ActionResult<PaginatedList<Clinic>>> GetAll(int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
        {
            orderBy = SetOrderBy(orderBy, ref isAsc);

            var pagination = new PaginatedList<Clinic>(orderBy, pageNumber, pageSize, isAsc);

            if(pagination.OrderBy == nameof(Clinic.IsFeatured))
                pagination.ThenOrderBy = nameof(Clinic.SortOrder);

            var list = await _service.GetAllAsync(pagination, PatientCityID, PatientID);

            InitLocalization(list);

            return Ok(list);
		}
        
        [AllowAnonymous]
		[HttpGet("getall/{serviceCategoryID:int}")]
		public async Task<ActionResult<PaginatedList<Clinic>>> GetAllByServiceCategory(int serviceCategoryID, 
            int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
		{
            orderBy = SetOrderBy(orderBy, ref isAsc);

            var list = await _service.GetAllAsync(serviceCategoryID, new PaginatedList<Clinic>(orderBy, pageNumber, pageSize, isAsc), PatientID);

            InitLocalization(list);

			return Ok(list);
		}

        [AllowAnonymous]
        [HttpGet("get-all-key-value/{cityID:int?}")]
        public async Task<ActionResult<NameValueModel>> GetAllByCity(int cityID = 0)
        {
            cityID = cityID > 0 ? cityID : PatientCityID;

            var list = await _service.GetAllByCityAsync(cityID);

            InitLocalization(list);

            var result = list.Select(c => new NameValueModel(c.ID,c.Name ));
            
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<ActionResult<PaginatedList<Clinic>>> Search(ClinicSearch search)
        {
            bool isAsc = true;
            search.OrderBy = SetOrderBy(search.OrderBy, ref isAsc);
            search.IsOrderAscending = isAsc;
            search.IsActive = true;

            var result = await _service.GetAllAsync(search);
			
            InitLocalization(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("search-by-service")]
        public async Task<ActionResult<List<Clinic>>> SearchByService(int serviceID, PaginationModel<Clinic> pagination)
        {
            bool isAsc = true;
            pagination.OrderBy = SetOrderBy(pagination.OrderBy, ref isAsc);
            pagination.IsOrderAscending = isAsc;

            var result = await _service.SearchAsync(serviceID, pagination, User.ID());

            InitLocalization(result);

            result.ForEach(c => c.ServiceClinic.Init(PatientLanguage));

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("get-by-region/{regionID:int}")]
        public async Task<ActionResult<List<Clinic>>> GetAllByRegion(int regionID)
        {
	        var result = await _service.GetAllAsync(regionID);
			
            InitLocalization(result);

	        return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("get-by-category/{categoryID:int}")]
        public async Task<ActionResult<PaginatedList<Clinic>>> GetAllByCategory(int categoryID, int pageSize, int pageNumber = 1, string orderBy = "ID", bool isAsc = true)
        {
            orderBy = SetOrderBy(orderBy, ref isAsc);
            
	        var pagination = new PaginatedList<Clinic>(orderBy, pageNumber, pageSize, isAsc);
	        var result = await _service.GetAllByCategoryAsync(categoryID, pagination, PatientCityID, PatientID);
			
            InitLocalization(result);

	        return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("AutoComplete")]
        public async Task<ActionResult<List<KeyValuePair<int, string>>>> AutoComplete(string term, int cityID = 0)
        {
            cityID = cityID > 0 ? cityID : PatientCityID;

            var list = await _service.GetAllAsync(new ClinicSearch { ClinicName = term, CityID = cityID, IsActive = true });

            InitLocalization(list);

            var result = list.Select(c => new KeyValuePair<int, string>(c.ID, c.Name));
            
            return Ok(result);
        }

		[AllowAnonymous]
		[HttpGet("get/{clinicQrKey}")]
        public async Task<ActionResult<Clinic>> Get(string clinicQrKey, string userIpAddress)
        {
            if (!Guid.TryParse(clinicQrKey, out Guid clinicKey))
                return BadRequest("Clinic not found");
            
            var clinic = await _service.GetAsync(clinicKey);

            InitLocalization(clinic);

            return Ok(clinic);
        }

		[AllowAnonymous]
		[HttpGet("getQrCode/{id:int}")]
        public async Task<ActionResult<string>> GetQrCode(int id)
        {
            var result = await _service.GetQrImageBase64Async(id);

            return !string.IsNullOrWhiteSpace(result) ? Ok(result) : BadRequest("Clinic not found");
        }


        private string SetOrderBy(string orderBy, ref bool isAsc)
        {
            if (orderBy.ToLower() == nameof(Clinic.ID).ToLower())
            {
                orderBy = nameof(Clinic.IsFeatured);
                isAsc = false;
            }

            return orderBy;
        }
	}
}