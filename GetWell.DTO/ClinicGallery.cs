using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicGallery
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsHidden { get; set; }
        public int SortOrder { get; set; }
        public bool IsThumbnail { get; set; }
        public bool IsMobileImage { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string TitleUz { get; set; }
        public string TitleEn { get; set; }
    }
}
