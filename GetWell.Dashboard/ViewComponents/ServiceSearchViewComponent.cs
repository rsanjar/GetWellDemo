using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class ServiceSearchViewComponent : ViewComponent
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public ServiceSearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(ServiceSearch search)
        {
            search.ServiceCategories = await _formCacheHelper.GetServiceCategoriesAsync(search.ServiceCategoryID);

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}
