using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicDoctor
    {
        public int ID { get; set; }
        public int ClinicID { get; set; }
        public int DoctorID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDisabled { get; set; }
    }
}
