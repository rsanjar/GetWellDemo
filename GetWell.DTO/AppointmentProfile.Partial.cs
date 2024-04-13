using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IAppointmentProfile))]
	[ModelMetadataType(typeof(IAppointmentProfile))]
    public partial class AppointmentProfile : BaseLocalizable<AppointmentProfile>, IAppointmentProfile
    {
        public int ServiceID { get; set; }

        public int ClinicID { get; set; }
        
        public int ClinicDoctorID { get; set; }

        public int ServiceClinicID { get; set; }
        
        public string ClinicName { get; set; }

        public string ServiceName { get; set; }
        
        public string DoctorFullName { get; set; }

        public string DoctorFullNameUz { get; set; }
        
        public string DoctorFullNameEn { get; set; }
        
        public string ServiceNameUz { get; set; }
        
        public string ServiceNameEn { get; set; }

        public string ClinicNameUz { get; set; }
        
        public string ClinicNameEn { get; set; }
        
        public string PatientFullName { get; set; }
    }
}
