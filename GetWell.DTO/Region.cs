using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Region
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }
        public int SortOrder { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
    }
}
