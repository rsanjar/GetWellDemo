using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicGallery))]
    [ModelMetadataType(typeof(IClinicGallery))]
    public partial class ClinicGallery : BaseLocalizable<ClinicGallery>, IClinicGallery
    {
        public IFormFile Image { get; set; }

        public string ImageFileName { get; set; }
        
        public string ClinicName { get; set; }
    }
}
