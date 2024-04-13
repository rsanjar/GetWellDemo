using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IPatientFavoriteDoctor))]
	[ModelMetadataType(typeof(IPatientFavoriteDoctor))]
    public partial class PatientFavoriteDoctor : BaseLocalizable<PatientFavoriteDoctor>, IPatientFavoriteDoctor
    {
        public double? Rating { get; set; }

	    public Doctor Doctor { get; set; }

        public DoctorProfile DoctorProfile { get; set; }
    }
}
