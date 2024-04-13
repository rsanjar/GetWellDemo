using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicPhone))]
    [ModelMetadataType(typeof(IClinicPhone))]
    public partial class ClinicPhone : IClinicPhone
    {
        
    }
}
