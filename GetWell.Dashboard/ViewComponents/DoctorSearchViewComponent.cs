using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class DoctorSearchViewComponent : ViewComponent 
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public DoctorSearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(DoctorSearch search)
        {
            search.Cities = await _formCacheHelper.GetCitiesAsync(search.CityID);
            search.Clinics = await _formCacheHelper.GetClinicsAsync(search.ClinicID);

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}