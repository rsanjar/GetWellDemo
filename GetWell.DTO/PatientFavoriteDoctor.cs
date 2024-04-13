using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class PatientFavoriteDoctor
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int ClinicDoctorID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int SortOrder { get; set; }
    }
}
