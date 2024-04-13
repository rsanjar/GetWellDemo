using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class ServiceSearchResultViewComponent : ViewComponent
    {
        #region ctor

        private readonly IServiceService _service;

        public ServiceSearchResultViewComponent(IServiceService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(ServiceSearch search)
        {
            var result = await _service.GetAllAsync(search);

            if (result == null)
                return new ContentViewComponentResult("No results found");

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}