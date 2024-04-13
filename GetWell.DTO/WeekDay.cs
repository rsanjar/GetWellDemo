using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class WeekDay
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
        public string ShortName { get; set; }
        public string ShortNameUz { get; set; }
        public string ShortNameEn { get; set; }
    }
}
