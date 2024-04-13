using System;
using System.IO;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.Dashboard.ViewComponents;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ServiceController : BaseController
    {
        #region ctor

        private readonly IServiceService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ServiceController> _logger;
        private readonly FormCacheHelper _formCacheHelper;

        public ServiceController(IServiceService service,
            IWebHostEnvironment webHostEnvironment,
            ILogger<ServiceController> logger,
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
            return await Task.FromResult(View(new ServiceSearch { IsActive = true }));
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(ServiceSearch search)
        {
            return await Task.FromResult(ViewComponent(typeof(ServiceSearchResultViewComponent), new { search }));
        }

        [HttpGet("[controller]/add")]
        public async Task<IActionResult> Add()
        {
            var item = new DTO.Service
            {
                ServiceCategories = await _formCacheHelper.GetServiceCategoriesAsync()
            };

            return await Task.FromResult(View("Save", item));
        }

        [HttpPost]
        public async Task<IActionResult> Add(DTO.Service item)
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
                return RedirectToAction("Index");

            item.ServiceCategories = await _formCacheHelper.GetServiceCategoriesAsync(item.ServiceCategoryID);

            return View("Save", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DTO.Service model)
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

        private string GetImageFilePath(DTO.Service item)
        {
            string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets\\images\\service");
            var fileName = string.Empty;

            if (item.ID <= 0 || item.IconImage == null)
                return fileName;

            fileName = $"{item.ID}{Path.GetExtension(item.IconImage.FileName)}".ToLower();
            item.IconUrl = $"{BaseHelpers.GetBaseUrl}/assets/images/service/{fileName}";

            string filePath = Path.Combine(imageFolder, fileName);

            return filePath;
        }

        private async Task SaveImage(DTO.Service item)
        {
            string filePath = GetImageFilePath(item);

            //profile picture is not a required field
            if (string.IsNullOrWhiteSpace(filePath))
                return;

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
                _logger.LogError(ex, "Error while saving the service icon image");
            }
        }

        #endregion
    }
}