using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class CalendarSearchViewComponent : ViewComponent 
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public CalendarSearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(CalendarSearch search)
        {
            search.Cities = await _formCacheHelper.GetCitiesAsync();

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}