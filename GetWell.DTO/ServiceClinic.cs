using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ServiceClinic
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int ServiceID { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public TimeSpan AverageDuration { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
