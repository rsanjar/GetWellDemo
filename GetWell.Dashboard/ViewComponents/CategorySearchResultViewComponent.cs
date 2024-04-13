using System.Threading.Tasks;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class CategorySearchResultViewComponent : ViewComponent
    {
        private readonly ICategoryService _service;

        public CategorySearchResultViewComponent(ICategoryService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _service.GetAllAsync(false);
            
            if (result == null)
                return new ContentViewComponentResult("No results found");
            
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}