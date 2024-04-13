using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IDoctorSpecialty))]
	[ModelMetadataType(typeof(IDoctorSpecialty))]
    public partial class DoctorSpecialty : IDoctorSpecialty
    {

    }
}
