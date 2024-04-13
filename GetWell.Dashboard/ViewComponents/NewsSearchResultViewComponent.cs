﻿using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class NewsSearchResultViewComponent : ViewComponent
    {
        #region ctor

        private readonly INewsService _service;

        public NewsSearchResultViewComponent(INewsService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(NewsSearch search)
        {
            var result = await _service.GetAllAsync(search);

            if (result == null)
                return new ContentViewComponentResult("No results found");

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}