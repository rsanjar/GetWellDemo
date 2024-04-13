using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IPatientProfile))]
    [ModelMetadataType(typeof(IPatientProfile))]
    public partial class PatientProfile : IPatientProfile
    {
        public int CityID { get; set; }

        public List<SelectListItem> Cities { get; set; }
    }
}
