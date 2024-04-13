using System.Collections.Generic;

namespace GetWell.API.Models;

public class GeneralSearchModel
{
    public List<NameValueModel> Services { get; set; }

    public List<NameValueModel> Clinics { get; set; }

    public List<NameValueModel> Doctors { get; set; }
}