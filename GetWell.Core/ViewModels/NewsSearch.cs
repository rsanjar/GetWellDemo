using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.Core.Enums;
using GetWell.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class NewsSearch : PaginationModel<News>
    {
        [JsonProperty]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; }

        [JsonProperty]
        [Display(Name = "В Архиве", Prompt = "В Архиве")]
        public bool IsDisabled { get; set; }

        [JsonProperty]
        [Display(Name = "Избранный", Prompt = "Избранный")]
        public bool IsFeatured { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Создания От", Prompt = "Дата Создания От")]
        public DateTime? DateCreatedFrom { get; set; }

        [JsonProperty]
        [Display(Name = "Дата Создания До", Prompt = "Дата Создания До")]
        public DateTime? DateCreatedTo { get; set; }
        
        public LanguageEnum Language { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Display(Name = "Язык", Prompt = "Язык")]
        public List<SelectListItem> Languages { get; set; }
    }
}