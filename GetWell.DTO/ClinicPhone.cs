using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicPhone
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public bool IsMain { get; set; }
        public bool? IsMobile { get; set; }
        public bool IsDisabled { get; set; }
        public int SortOrder { get; set; }
        public int ClinicID { get; set; }
    }
}
