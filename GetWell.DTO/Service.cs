using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Service
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ServiceCategoryID { get; set; }
        public bool? IsActive { get; set; }
        public int SortOrder { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
        public string DescriptionUz { get; set; }
        public string DescriptionEn { get; set; }
        public string IconUrl { get; set; }
    }
}
