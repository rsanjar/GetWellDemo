using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicReview
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int Rating { get; set; }
        public string ReviewTitle { get; set; }
        public string Review { get; set; }
        public int ClinicID { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? ReviewLanguage { get; set; }
    }
}
