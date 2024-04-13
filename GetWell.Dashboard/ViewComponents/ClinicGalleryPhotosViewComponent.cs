using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Threading.Tasks;

namespace GetWell.Dashboard.ViewComponents
{
    public class ClinicGalleryPhotosViewComponent : ViewComponent
    {
        #region ctor

        private readonly IClinicGalleryService _service;
        private readonly IClinicService _clinicService;

        public ClinicGalleryPhotosViewComponent(IClinicGalleryService clinicGalleryService,
            IClinicService clinicService)
        {
            _service = clinicGalleryService;
            _clinicService = clinicService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(int clinicID)
        {
            var result = await _clinicService.GetAsync(clinicID);
            
            if (result == null)
                return new ContentViewComponentResult("No results found");

            result.Gallery = await _service.GetAllAsync(clinicID);
            
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}