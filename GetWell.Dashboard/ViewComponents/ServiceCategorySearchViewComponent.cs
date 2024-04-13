using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.Cache;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class ServiceCategorySearchViewComponent : ViewComponent
    {
        #region ctor

        private readonly FormCacheHelper _formCacheHelper;

        public ServiceCategorySearchViewComponent(
            FormCacheHelper formCacheHelper)
        {
            _formCacheHelper = formCacheHelper;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(ServiceCategorySearch search)
        {
            search.Categories = await _formCacheHelper.GetCategoriesAsync(search.CategoryID);

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(search);
        }
    }
}