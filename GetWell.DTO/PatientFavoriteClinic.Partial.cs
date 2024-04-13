using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IPatientFavoriteClinic))]
	[ModelMetadataType(typeof(IPatientFavoriteClinic))]
    public partial class PatientFavoriteClinic : BaseLocalizable<PatientFavoriteClinic>, IPatientFavoriteClinic
    {
        public double? Rating { get; set; }

        public Clinic Clinic { get; set; }
    }
}
