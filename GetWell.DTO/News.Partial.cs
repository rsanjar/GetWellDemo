using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(INews))]
    [ModelMetadataType(typeof(INews))]
    public partial class News : INews
    {
        public IFormFile Image { get; set; }
        
        public string Language
        {
            get
            {
                switch (NewsLanguage)
                {
                    case 0 : return "Русский";
                    case 1 : return "O'zbekcha";
                    case 2 : return "English";
                    default : return " - ";
                }
            }
        }

        public List<SelectListItem> Languages { get; set; }

        public News InitUpdate(News item)
        {
            Title = item.Title;
            Body = WebUtility.HtmlEncode(item.Body);
            IsDisabled = item.IsDisabled;
            IsFeatured = item.IsFeatured;
            DateUpdated = DateTime.UtcNow;
            NewsLanguage = item.NewsLanguage;

            if(!string.IsNullOrWhiteSpace(item.BannerUrl))
                BannerUrl = item.BannerUrl;

            return this;
        }
    }
}
