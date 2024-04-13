using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicDiscount
    {
        public int ID { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int ServiceClinicID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int SortOrder { get; set; }
        public string TitleUz { get; set; }
        public string TitleEn { get; set; }
        public string BodyUz { get; set; }
        public string BodyEn { get; set; }
    }
}
