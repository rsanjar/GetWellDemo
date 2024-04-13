using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ServiceClinicDoctor
    {
        public int ID { get; set; }
        public int ServiceClinicID { get; set; }
        public int ClinicDoctorID { get; set; }
        public bool? IsActive { get; set; }
        public TimeSpan? AverageDuration { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string DescriptionUz { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
