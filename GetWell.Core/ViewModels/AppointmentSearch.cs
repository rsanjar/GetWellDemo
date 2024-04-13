using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AppointmentSearch : PaginationModel<Appointment>
    {
        [JsonProperty]
        [Display(Name = "Клиника", Prompt = "Клиника")]
        public int? ClinicID { get; set; }

        [JsonProperty]
        [Display(Name = "Название Клиники", Prompt = "Название Клиники")]
        public string ClinicName { get; set; }

        [JsonProperty]
        [Display(Name = "Город", Prompt = "Город")]
        public int CityID { get; set; }

        [JsonProperty]
        [Display(Name = "Регион", Prompt = "Регион")]
        public int? RegionID { get; set; }

        [JsonProperty]
        [Display(Name = "Врач", Prompt = "Врач")]
        public int? ClinicDoctorID { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Записи От", Prompt = "Дата Записи От")]
        public DateTime? AppointmentDateStart { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Записи До", Prompt = "Дата Записи До")]
        public DateTime? AppointmentDateEnd { get; set; }

        [JsonProperty]
        [Display(Name = "ПОДТВЕРЖДЕН!", Prompt = "ПОДТВЕРЖДЕН!")]
        public bool? IsDoctorConfirmed { get; set; }

        [JsonProperty]
        [Display(Name = "Отменен", Prompt = "Отменен")]
        public bool? IsCanceled { get; set; }

        [JsonProperty]
        [Display(Name = "Законченная Встреча", Prompt = "Законченная Встреча")]
        public bool? IsActive { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Cities { get; set; }
    }
}