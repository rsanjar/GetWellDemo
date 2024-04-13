using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IRegion))]
	[ModelMetadataType(typeof(IRegion))]
    public partial class Region : BaseLocalizable<Region>, IRegion
    {

    }
}
