using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ZipCode
    {
        public int ID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int RegionID { get; set; }
    }
}
