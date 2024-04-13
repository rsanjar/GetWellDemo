using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ServiceCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
        public string DescriptionUz { get; set; }
        public string DescriptionEn { get; set; }
        public string IconUrl { get; set; }
        public string HexColor { get; set; }
        public int CategoryID { get; set; }
        public int TitleID { get; set; }
    }
}
