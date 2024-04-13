using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.ViewComponents
{
    public class NewsSearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(NewsSearch search)
        {
	        search.OrderBy = nameof(News.DateCreated);
	        search.IsOrderAscending = false;

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return await Task.FromResult(View(search));
        }
    }
}
