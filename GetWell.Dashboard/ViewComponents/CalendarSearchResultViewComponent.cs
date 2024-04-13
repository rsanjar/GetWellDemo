using System.Threading.Tasks;
using GetWell.Core.ViewModels;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GetWell.Dashboard.ViewComponents
{
    public class CalendarSearchResultViewComponent : ViewComponent 
    {
        #region ctor

        private readonly IAppointmentService _service;

        public CalendarSearchResultViewComponent(IAppointmentService service)
        {
            _service = service;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(CalendarSearch search)
        {
            var result = await _service.GetAllAsync(new AppointmentSearch
            {
                ClinicID = search.ClinicID,
                ClinicDoctorID = search.ClinicDoctorID,
                CityID = search.CityID,
                AppointmentDateStart = search.Start,
                AppointmentDateEnd = search.End,
                PageSize = 99998
            });

            if (result == null)
                return new ContentViewComponentResult("No results found");

            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(result);
        }
    }
}