using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IZipCode))]
	[ModelMetadataType(typeof(IZipCode))]
    public partial class ZipCode : IZipCode
    {
    }
}
