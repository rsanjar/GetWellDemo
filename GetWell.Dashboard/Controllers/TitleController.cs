using GetWell.Core;
using GetWell.Dashboard.ViewComponents;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GetWell.Core.Enums;
using GetWell.DTO;

namespace GetWell.Dashboard.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class TitleController : BaseController
{
    #region ctor

    private readonly ITitleService _service;

    public TitleController(ITitleService service)
    {
        _service = service;
    }

    #endregion

    [HttpGet("[controller]")]
    public async Task<IActionResult> Index()
    {
        return await Task.FromResult(View());
    }

    [HttpPost]
    public async Task<IActionResult> SearchResult()
    {
        return await Task.FromResult(ViewComponent(typeof(TitleSearchResultViewComponent)));
    }

    [HttpGet("[controller]/add")]
    public async Task<IActionResult> Add()
    {
        return await Task.FromResult(View("Save", new Title()));
    }

    [HttpPost]
    public async Task<IActionResult> Add(Title item)
    {
        if (!ModelState.IsValid)
            return await ContentResultAsync(Crud.ValidationError);

        var result = await _service.SaveAsync(item.InitUpdate(item));

        return await ContentResultAsync(result.MessageKey);
    }

    [HttpGet("[controller]/edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetAsync(id);

        if (item == null)
            return RedirectToAction("Index", "Title");

        return View("Save", item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Title model)
    {
        CrudResponse result = new CrudResponse(Crud.Error);

        if (!ModelState.IsValid)
            return await ContentResultAsync(Crud.ValidationError);

        var item = await _service.GetAsync(model.ID);

        if (item != null)
            result = await _service.UpdateAsync(item.InitUpdate(model));

        return await ContentResultAsync(result.MessageKey);
    }
}