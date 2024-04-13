using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IClinicRole))]
	[ModelMetadataType(typeof(IClinicRole))]
    public partial class ClinicRole : IClinicRole
    {
    }
}
