using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class DoctorPhone
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public bool? IsMobile { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsWorkPhone { get; set; }
        public bool? IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int DoctorID { get; set; }
    }
}
