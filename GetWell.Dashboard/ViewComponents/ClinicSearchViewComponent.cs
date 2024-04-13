using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class ClinicSearchViewComponent : ViewComponent
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public ClinicSearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(ClinicSearch search)
        {
            search.Cities = await _formCacheHelper.GetCitiesAsync(search.CityID);
            search.OrderBy = nameof(Clinic.DateCreated);
            search.IsOrderAscending = false;

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}