using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IWeekDay))]
	[ModelMetadataType(typeof(IWeekDay))]
    public partial class WeekDay : BaseLocalizable<WeekDay>, IWeekDay
    {

    }
}
