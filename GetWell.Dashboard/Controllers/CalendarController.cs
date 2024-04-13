using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Helpers;
using GetWell.Core.ViewModels;
using GetWell.Dashboard.ViewComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.Dashboard.Controllers;

public class CalendarController : BaseController
{
    [HttpGet("[controller]")]
    [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
    public async Task<IActionResult> Index()
    {
        var search = new CalendarSearch();

        if (User.IsInRole(UserRoles.Clinic))
            search.ClinicID = User.ID();
            
        if (User.IsInRole(UserRoles.Doctor))
            search.ClinicDoctorID = User.ID();
            
        return await Task.FromResult(View(search));
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.AdminOrClinicOrDoctor)]
    public async Task<IActionResult> SearchResult(CalendarSearch search)
    {
        if (User.IsInRole(UserRoles.Clinic))
            search.ClinicID = User.ID();
            
        if (User.IsInRole(UserRoles.Doctor))
            search.ClinicDoctorID = User.ID();

        return await Task.FromResult(ViewComponent(typeof(CalendarSearchResultViewComponent), new { search }));
    }
}