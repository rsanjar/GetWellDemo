using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ClinicSearch : PaginationModel<Clinic>
    {
        [JsonProperty]
        [Display(Name = "Название Клиники", Prompt = "Название Клиники")]
        public string ClinicName { get; set; }

        [JsonProperty]
        [Display(Name = "Город", Prompt = "Город")]
        public int CityID { get; set; }

        [JsonProperty]
        [Display(Name = "Район", Prompt = "Район")]
        public int? RegionID { get; set; }

        [JsonProperty]
        [Display(Name = "Активирован", Prompt = "Активирован")]
        public bool? IsActive { get; set; }

        [JsonProperty]
        [Display(Name = "Избранный", Prompt = "Избранный")]
        public bool? IsFeatured { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Регистрации От", Prompt = "Дата Регистрации От")]
        public DateTime? RegistrationDateStart { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Регистрации До", Prompt = "Дата Регистрации До")]
        public DateTime? RegistrationDateEnd { get; set; }
        
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Cities { get; set; }
    }
}