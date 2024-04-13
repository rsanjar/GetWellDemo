using System.Drawing;
using System.Threading.Tasks;
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

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class NewsController : BaseImageController
    {
        #region ctor

        private readonly INewsService _service;
        private readonly FormCacheHelper _formCacheHelper;

        public NewsController(
            INewsService service,
            IWebHostEnvironment webHostEnvironment,
            FormCacheHelper formCacheHelper) : base(webHostEnvironment)
        {
            _service = service;
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var search = new NewsSearch
            {
                IsDisabled = false, 
                Languages = await _formCacheHelper.GetLanguagesAsync(),
                Language = LanguageEnum.Default
            };

            return await Task.FromResult(View(search));
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(NewsSearch search)
        {
            return await Task.FromResult(ViewComponent(typeof(NewsSearchResultViewComponent), new { search }));
        }

        [HttpGet("[controller]/add")]
        public async Task<IActionResult> Add()
        {
            var item = new News
            {
                Languages = await _formCacheHelper.GetLanguagesAsync()
            };

            return await Task.FromResult(View("Save", item));
        }

        [HttpGet("[controller]/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetAsync(id);
            
            if (item == null)
                return RedirectToAction("Index", "News");
            
            item.Languages = await _formCacheHelper.GetLanguagesAsync();
            
            return View("Save", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(News model)
        {
            CrudResponse result = new CrudResponse(Crud.Error);

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var item = await _service.GetAsync(model.ID);

            if (item != null)
            {
                model.BannerUrl = await SaveJpegImage(model.ID, model.Image, new Size(800, 800), ImageFolderEnum.News);

                result = await _service.UpdateAsync(item.InitUpdate(model));
            }
            
            return await ContentResultAsync(result.MessageKey);
        }

        [HttpPost]
        public async Task<IActionResult> Add(News model)
        {
            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);
            
            var result = await _service.SaveAsync(model);

            if (result.IsSuccess)
            {
                model.BannerUrl = await SaveJpegImage(model.ID, model.Image, new Size(800, 800), ImageFolderEnum.News);
                result = await _service.UpdateAsync(model.InitUpdate(model));
            }

            return await ContentResultAsync(result.MessageKey);
        }
    }
}