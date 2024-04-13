using System.Threading.Tasks;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class TitleSearchResultViewComponent : ViewComponent
    {
        #region ctor

        private readonly ITitleService _service;

        public TitleSearchResultViewComponent(ITitleService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _service.GetAllAsync();

            if (result == null)
                return new ContentViewComponentResult("No results found");

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}