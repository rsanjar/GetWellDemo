using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicDoctorEvent))]
    [ModelMetadataType(typeof(IClinicDoctorEvent))]
    public partial class ClinicDoctorEvent : IClinicDoctorEvent
    {
        
    }
}
