using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(ICountry))]
	[ModelMetadataType(typeof(ICountry))]
    public partial class Country : BaseLocalizable<Country>, ICountry
    {
        
    }
}
