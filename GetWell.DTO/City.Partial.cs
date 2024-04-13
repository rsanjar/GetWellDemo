using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(ICity))]
	[ModelMetadataType(typeof(ICity))]
    public partial class City : BaseLocalizable<City>, ICity
    {

    }
}
