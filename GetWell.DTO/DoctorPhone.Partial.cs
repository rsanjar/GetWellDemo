using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IDoctorPhone))]
    [ModelMetadataType(typeof(IDoctorPhone))]
    public partial class DoctorPhone : IDoctorPhone
    {
        
    }
}
