using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace GetWell.Dashboard.Cache
{
    public class FormCacheHelper
    {
        #region ctor
        
        private readonly ICityService _cityService;
        private readonly ICategoryService _categoryService;
        private readonly ITitleService _titleService;
        private readonly IServiceCategoryService _serviceCategoryService;
        private readonly IServiceClinicService _serviceClinicService;
        private readonly IClinicService _clinicService;
        private readonly IClinicDoctorService _clinicDoctorService;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly TimeSpan _cacheTimeSpan;

        public FormCacheHelper(
            ICityService cityService,
            ICategoryService categoryService,
            ITitleService titleService,
            IServiceCategoryService serviceCategoryService,
            IClinicService clinicService,
            IMemoryCache cache, 
            IHttpContextAccessor httpContextAccessor, 
            IServiceClinicService serviceClinicService, 
            IClinicDoctorService clinicDoctorService)
        {
            _cityService = cityService;
            _categoryService = categoryService;
            _titleService = titleService;
            _serviceCategoryService = serviceCategoryService;
            _clinicService = clinicService;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _serviceClinicService = serviceClinicService;
            _clinicDoctorService = clinicDoctorService;

            _cacheTimeSpan = TimeSpan.FromSeconds(5);
        }

        #endregion

        public async Task<List<SelectListItem>> GetSelectAsync(CacheKeys key, int selectedId = -1)
        {
            return key switch
            {
                CacheKeys.CitiesSelectList => await GetCitiesAsync(selectedId),
                CacheKeys.CategoriesSelectList => await GetCategoriesAsync(selectedId),
                CacheKeys.TitleSelectList => await GetServiceCategoriesAsync(selectedId),
                CacheKeys.ServiceCategoriesSelectList => await GetTitlesAsync(selectedId),
                CacheKeys.ClinicSelectList => await GetClinicsAsync(selectedId),
                CacheKeys.LanguageList => await GetLanguagesAsync(selectedId),
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }

        public async Task<List<SelectListItem>> GetCitiesAsync(int selectedId = -1, string defaultText = "Выберите Город")
        {
            var list = await _cache.GetOrCreateAsync(CacheKeys.CitiesSelectList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

                return await _cityService.GetAllAsync(1, new PaginatedList<City>("Name", 1, 200));
            });

            var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Name));

            return selectList;
        }

        public async Task<List<SelectListItem>> GetCategoriesAsync(int selectedId = -1, string defaultText = "Выберите Тип Клиники")
        {
            var list = await _cache.GetOrCreateAsync(CacheKeys.CategoriesSelectList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

                return await _categoryService.GetAllAsync(1, 200, "Name"); //there are only a few clinic categories, no need for pagination
            });

            var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Name));

            return selectList;
        }

        public async Task<List<SelectListItem>> GetServiceCategoriesAsync(int selectedId = -1, string defaultText = "Выберите Тип Услуги")
        {
	        var httpContext = _httpContextAccessor.HttpContext;
	        bool isClinic = httpContext?.User.Role() == UserRoles.Clinic;

	        var list = await _cache.GetOrCreateAsync(
		        $"{(isClinic ? httpContext.User.ID() : string.Empty)}{CacheKeys.ServiceCategoriesSelectList}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

                if(httpContext != null && isClinic)
					return await _serviceCategoryService.GetAllAsync(httpContext.User.ID(), false);

                return await _serviceCategoryService.GetAllAsync();
            });

            var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Name));

            return selectList;
        } 

        public async Task<List<SelectListItem>> GetClinicServicesAsync(int selectedId = -1, int? serviceCategoryId = null, string defaultText = "Выберите Услугу")
        {
	        var httpContext = _httpContextAccessor.HttpContext;
	        bool isClinic = httpContext?.User.Role() == UserRoles.Clinic;
            
	        var list = await _cache.GetOrCreateAsync(
		        $"{serviceCategoryId}{(isClinic ? httpContext.User.ID() : string.Empty)}{CacheKeys.ClinicServiceList}", async entry =>
		        {
			        entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

			        if(httpContext != null && isClinic)
				        return await _serviceClinicService.GetAllByServiceCategoryAsync(httpContext.User.ID(), serviceCategoryId ?? 0);

			        return new List<ServiceClinic>();
		        });

	        var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Service.Name));

	        return selectList;
        }

        public async Task<List<SelectListItem>> GetClinicServiceDoctorsAsync(int serviceClinicId, int selectedId = -1, string defaultText = "Выберите Врача")
        {
	        var httpContext = _httpContextAccessor.HttpContext;
	        bool isClinic = httpContext?.User.Role() == UserRoles.Clinic;

	        var list = await _cache.GetOrCreateAsync(
		        $"{serviceClinicId}{(isClinic ? httpContext.User.ID() : string.Empty)}{CacheKeys.ClinicServiceDoctorList}", async entry =>
		        {
			        entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

			        if(httpContext != null && isClinic)
				        return await _clinicDoctorService.GetAllByServiceAsync(serviceClinicId);

			        return new List<ClinicDoctor>();
		        });

	        var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Doctor.FullName));

	        return selectList;
        }

        public async Task<List<SelectListItem>> GetTitlesAsync(int selectedId = -1, string defaultText = "Выберите Тип Врача")
        {
            var list = await _cache.GetOrCreateAsync(CacheKeys.TitleSelectList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

                return await _titleService.GetAllAsync(1, 200, "Name");
            });

            var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Name));

            return selectList;
        }

        public async Task<List<SelectListItem>> GetClinicsAsync(int selectedId = -1, string defaultText = "Выберите Клинику")
        {
            var list = await _cache.GetOrCreateAsync(CacheKeys.ClinicSelectList, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

                return await _clinicService.GetAllAsync(1, 200, "Name");
            });

            var selectList = SelectListItems(selectedId, defaultText, list.ToDictionary(c => c.ID, k => k.Name));

            return selectList;
        }

        public async Task<List<SelectListItem>> GetLanguagesAsync(int selectedId = -1, string defaultText = "Выберите Язык")
        {
            List<SelectListItem> result = new()
            {
                new(defaultText, ((int)LanguageEnum.Default).ToString(), true),
                new("Русский", ((int)LanguageEnum.Ru).ToString()),
                new("O'zbekcha", ((int)LanguageEnum.Uz).ToString()),
                new("English", ((int)LanguageEnum.En).ToString())
            };

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Clears all cache by CacheKeys
        /// </summary>
        public void Reset()
        {
            foreach (var i in Enum.GetValues<CacheKeys>())
            {
                _cache.Remove(i);
            }
        }

        #region Private Methods

        public static List<SelectListItem> SelectListItems(int selectedId, string defaultText, Dictionary<int, string> list)
        {
            var selectList = new List<SelectListItem>();
            selectList.Insert(0, new SelectListItem(defaultText, "-1", selectedId <= 0));
            selectList.AddRange(list.Select(c => new SelectListItem(c.Value, c.Key.ToString(), c.Key == selectedId)));

            return selectList;
        }

        #endregion
        
    }
}