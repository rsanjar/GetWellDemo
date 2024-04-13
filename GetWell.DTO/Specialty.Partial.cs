using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(ISpecialty))]
	[ModelMetadataType(typeof(ISpecialty))]
    public partial class Specialty : BaseLocalizable<Specialty>, ISpecialty
    {
    }
}
