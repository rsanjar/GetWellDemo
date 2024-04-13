using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PatientSearch : PaginationModel<Patient>
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
        [Display(Name = "Доктор", Prompt = "Доктор")]
        public int DoctorID { get; set; }

        [JsonProperty]
        [Display(Name = "Имя", Prompt = "Имя")]
        public string FirstName { get; set; }

        [JsonProperty]
        [Display(Name = "Фамилия", Prompt = "Фамилия")]
        public string LastName { get; set; }

        [JsonProperty]
        [Display(Name = "Отчество", Prompt = "Отчество")]
        public string MiddleName { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Рождения От", Prompt = "Дата Рождения От")]
        public DateTime? DateOfBirthFrom { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Рождения До", Prompt = "Дата Рождения До")]
        public DateTime? DateOfBirthTo { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Cities { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Clinics { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<SelectListItem> Doctors { get; set; }
    }
}