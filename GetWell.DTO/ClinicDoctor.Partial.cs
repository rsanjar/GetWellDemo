using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IClinicDoctor))]
	[ModelMetadataType(typeof(IClinicDoctor))]
    public partial class ClinicDoctor : IClinicDoctor
    {
        public int ServiceClinicDoctorID { get; set; }

	    public Doctor Doctor { get; set; } = new();

        public DoctorProfile DoctorProfile { get; set; }
        
        public double? Rating { get; set; }

        public bool IsPatientFavorite { get; set; }
    }
}
