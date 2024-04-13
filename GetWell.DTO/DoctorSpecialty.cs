using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class DoctorSpecialty
    {
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public int SpecialtyID { get; set; }
        public bool? IsActive { get; set; }
    }
}
