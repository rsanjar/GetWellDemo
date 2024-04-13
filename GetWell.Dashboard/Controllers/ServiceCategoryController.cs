using System;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GetWell.Core.Helpers;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ServiceCategoryController : BaseController
    {
        #region ctor

        private readonly IServiceCategoryService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ServiceCategoryController> _logger;
        private readonly FormCacheHelper _formCacheHelper;

        public ServiceCategoryController(IServiceCategoryService service, 
            IWebHostEnvironment webHostEnvironment,
            ILogger<ServiceCategoryController> logger,
            FormCacheHelper formCacheHelper)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        [HttpGet("[controller]")]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View(new ServiceCategorySearch { IsActive = true }));
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(ServiceCategorySearch search)
        {
            return await Task.FromResult(ViewComponent(typeof(ServiceCategorySearchResultViewComponent), new {search}));
        }

        [HttpGet("[controller]/add")]
        public async Task<IActionResult> Add()
        {
            var item = new ServiceCategory
            {
                Categories = await _formCacheHelper.GetCategoriesAsync(),
                Titles = await _formCacheHelper.GetTitlesAsync()
            };

            return await Task.FromResult(View("Save", item));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServiceCategory item)
        {
            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var result = await _service.SaveAsync(item);

            if (result.IsSuccess)
            {
                await SaveImage(item);
                result = await _service.UpdateAsync(item);
            }

            return await ContentResultAsync(result.MessageKey);
        }

        [HttpGet("[controller]/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetAsync(id);

            if (item == null)
                return RedirectToAction("Index", "ServiceCategory");

            item.Categories = await _formCacheHelper.GetCategoriesAsync();
            item.Titles = await _formCacheHelper.GetTitlesAsync();

            return View("Save", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceCategory model)
        {
            CrudResponse result = new CrudResponse(Crud.Error);

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var item = await _service.GetAsync(model.ID);

            if (item != null)
            {
                await SaveImage(model);
                result = await _service.UpdateAsync(item.InitUpdate(model));
            }
            
            return await ContentResultAsync(result.MessageKey);
        }

        #region Private Methods

        private string GetImageFilePath(ServiceCategory item)
        {
            string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets\\images\\service-category");
            var fileName = string.Empty;

            if (item.ID <= 0 || item.IconImage == null)
                return fileName;

            fileName = $"{item.ID}{Path.GetExtension(item.IconImage.FileName)}".ToLower();
            item.IconUrl = $"{BaseHelpers.GetBaseUrl}/assets/images/service-category/{fileName}";

            string filePath = Path.Combine(imageFolder, fileName);

            return filePath;
        }

        private async Task SaveImage(ServiceCategory item)
        {
            string filePath = GetImageFilePath(item);

            //profile picture is not a required field
            if (string.IsNullOrWhiteSpace(filePath)) return;

            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await item.IconImage.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while saving the service category icon image");
            }
        }

        #endregion
    }
}