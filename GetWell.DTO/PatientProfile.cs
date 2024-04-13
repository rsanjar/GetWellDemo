using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class PatientProfile
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public int? RegionID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsMale { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
