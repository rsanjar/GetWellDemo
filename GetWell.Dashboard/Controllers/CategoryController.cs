using System.Drawing;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : BaseImageController
    {
        #region ctor

        private readonly ICategoryService _service;

        public CategoryController(ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            _service = categoryService;
        }

        #endregion

        [HttpGet("[controller]")]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult()
        {
            return await Task.FromResult(ViewComponent(typeof(CategorySearchResultViewComponent)));
        }

        [HttpGet("[controller]/add")]
        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View("Save", new Category()));
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category item)
        {
            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var result = await _service.SaveAsync(item.InitSave());

            if (result.IsSuccess)
            {
                item.IconUrl = await SavePngImage(item.ID, item.IconImage, new Size(144, 144), ImageFolderEnum.Category);
                
                result = await _service.UpdateAsync(item);
            }

            return await ContentResultAsync(result.MessageKey);
        }

        [HttpGet("[controller]/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetAsync(id);

            if (item == null)
                return RedirectToAction("Index", "Category");

            return View("Save", item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            CrudResponse result = new CrudResponse(Crud.Error);

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            var item = await _service.GetAsync(model.ID);

            if (item != null)
            {
                model.IconUrl = await SavePngImage(item.ID, model.IconImage, new Size(144, 144), ImageFolderEnum.Category);
                
                result = await _service.UpdateAsync(item.InitUpdate(model));
            }
            
            return await ContentResultAsync(result.MessageKey);
        }
    }
}