using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ClinicDiscountSearch : PaginationModel<ClinicDiscount>
    {
        [JsonProperty]
        [Display(Name = "Город", Prompt = "Город")]
        public int CityID { get; set; }

        [JsonProperty]
        [Display(Name = "Регион", Prompt = "Регион")]
        public int? RegionID { get; set; }

        [JsonProperty]
        [Display(Name = "Клиника", Prompt = "Клиника")]
        public int ClinicID { get; set; }
        
        [JsonProperty]
        [Display(Name = "Активен", Prompt = "Активен")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Cities { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Clinics { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> ClinicServices { get; set; }
    }
}