using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class ClinicSearchResultViewComponent : ViewComponent
    {
        #region ctor

        private readonly IClinicService _service;

        public ClinicSearchResultViewComponent(IClinicService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(ClinicSearch search)
        {
            var result = await _service.GetAllAsync(search);

            if (result == null)
                return new ContentViewComponentResult("No results found");
            
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}