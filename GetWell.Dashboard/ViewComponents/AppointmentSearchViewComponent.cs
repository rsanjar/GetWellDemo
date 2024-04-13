using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class AppointmentSearchViewComponent : ViewComponent
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public AppointmentSearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(AppointmentSearch search)
        {
            search.Cities = await _formCacheHelper.GetCitiesAsync(search.CityID);
            search.OrderBy = nameof(Appointment.DateCreated);
            search.IsOrderAscending = false;
            
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}
