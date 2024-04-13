using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class DoctorProfile
    {
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public int PreferredLanguageID { get; set; }
        public string About { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string HomeAddress { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public int RegionID { get; set; }
        public bool IsMale { get; set; }
        public string MedicalTitle { get; set; }
        public string TaxpayerID { get; set; }
        public string AboutUz { get; set; }
        public string AboutEn { get; set; }
        public string MedicalTitleUz { get; set; }
        public string MedicalTitleEn { get; set; }
    }
}
