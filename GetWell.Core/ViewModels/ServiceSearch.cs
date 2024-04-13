using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServiceSearch : PaginationModel<Service>
    {
        [JsonProperty]
        [Display(Name = "Название Услуги", Prompt = "Название Услуги")]
        public string Name { get; set; }

        [JsonProperty]
        [Display(Name = "Активный", Prompt = "Активный")]
        public bool IsActive { get; set; }

        [JsonProperty]
        [Display(Name = "Тип Клиники", Prompt = "Тип Клиники")]
        public int ServiceCategoryID { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> ServiceCategories { get; set; }
    }
}