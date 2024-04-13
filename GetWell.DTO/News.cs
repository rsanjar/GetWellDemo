using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string BannerUrl { get; set; }
        public int? NewsLanguage { get; set; }
    }
}
