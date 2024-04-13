using System.Drawing;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class ClinicDiscountController : BaseImageController
{
    #region ctor

    private readonly ICityService _cityService;
    private readonly IClinicDiscountService _service;
    private readonly IClinicService _clinicService;
    private readonly IServiceClinicService _serviceClinicService;

    public ClinicDiscountController(IClinicDiscountService service,
        IClinicService clinicService,
        IWebHostEnvironment webHostEnvironment,
        ICityService cityService, 
        IServiceClinicService serviceClinicService) : base(webHostEnvironment)
    {
        _service = service;
        _clinicService = clinicService;
        _cityService = cityService;
        _serviceClinicService = serviceClinicService;
    }

    #endregion

    [HttpGet("[controller]")]
    public async Task<IActionResult> Index()
    {
        return await Task.FromResult(View(new ClinicDiscountSearch()));
    }

    [HttpPost]
    public async Task<IActionResult> SearchResult(ClinicDiscountSearch search)
    {
        if (User.IsInRole(UserRoles.Clinic))
            search.ClinicID = User.ID();

        return await Task.FromResult(ViewComponent(typeof(ClinicDiscountSearchResultViewComponent), new { search }));
    }

    [HttpGet("[controller]/add")]
    public async Task<IActionResult> Add()
    {
        return await Task.FromResult(View("Save", new ClinicDiscount()));
    }

    [HttpPost]
    public async Task<IActionResult> Add(ClinicDiscount item)
    {
        if (!ModelState.IsValid)
            return await ContentResultAsync(Crud.ValidationError);

        item.ImageUrl = string.Empty;

        var result = await _service.SaveAsync(item);

        if (result.IsSuccess)
        {
            item.ImageUrl = await SaveJpegImage(item.ID, item.DiscountImage, new Size(800, 800), ImageFolderEnum.ClinicDiscount, item.ClinicID);

            result = await _service.UpdateAsync(item);
        }

        return await ContentResultAsync(result.MessageKey);
    }

    [HttpGet("[controller]/edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetAsync(id);

        if (item == null)
            return RedirectToAction("Index", "ClinicDiscount");

        var serviceClinic = await _serviceClinicService.GetAsync(item.ServiceClinicID);

        item.ClinicID = serviceClinic.ClinicID;

        var clinic = await _clinicService.GetAsync(item.ClinicID);
        var city = await _cityService.GetByRegionAsync(clinic.RegionID);

        item.ClinicCityID = city.ID;
        item.ClinicCityRegionID = clinic.RegionID;

        return View("Save", item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClinicDiscount model)
    {
        CrudResponse result = new CrudResponse(Crud.Error);

        if (!ModelState.IsValid)
            return await ContentResultAsync(Crud.ValidationError);

        var item = await _service.GetAsync(model.ID);

        if (item != null)
        {
            model.ImageUrl = await SaveJpegImage(model.ID, model.DiscountImage, new Size(800, 800), ImageFolderEnum.ClinicDiscount, model.ClinicID);

            result = await _service.UpdateAsync(item.InitUpdate(model));
        }

        return await ContentResultAsync(result.MessageKey);
    }
}