using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.Core.ViewModels;

public class CalendarSearch
{
    [Display(Name = "Город", Prompt = "Город")]
    public int CityID { get; set; }

    [Display(Name = "Клиника", Prompt = "Клиника")]
    public int? ClinicID { get; set; }

    [Display(Name = "Врач", Prompt = "Врач")]
    public int? ClinicDoctorID { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public List<SelectListItem> Cities { get; set; }
}