using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class AppointmentSearchResultViewComponent : ViewComponent
    {
        #region ctor

        private readonly IAppointmentService _service;

        public AppointmentSearchResultViewComponent(IAppointmentService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(AppointmentSearch search)
        {
            var result = await _service.GetAllAsync(search);

            if (result == null)
                return new ContentViewComponentResult("No results found");

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}