using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IClinicReview))]
	[ModelMetadataType(typeof(IClinicReview))]
    public partial class ClinicReview : IClinicReview
    {
    }
}
