using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IDoctorProfile))]
    [ModelMetadataType(typeof(IDoctorProfile))]
    public partial class DoctorProfile : BaseLocalizable<DoctorProfile>, IDoctorProfile
    {
        public IFormFile ProfilePicture { get; set; }

        public int CityID { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public DoctorProfile InitUpdate(DoctorProfile item)
        {
            About = item.About;
            AboutUz = item.AboutUz;
            AboutEn = item.AboutEn;
            ProfilePictureUrl = item.ProfilePictureUrl;
            HomeAddress = item.HomeAddress;
            Street = item.Street;
            District = item.District;
            RegionID = item.RegionID;
            IsMale = item.IsMale;
            MedicalTitle = item.MedicalTitle;
            TaxpayerID = item.TaxpayerID;
            
            return this;
        }
    }
}
