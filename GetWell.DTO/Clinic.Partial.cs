using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinic))]
    [ModelMetadataType(typeof(IClinic))]
    public partial class Clinic : BaseLocalizable<Clinic>, IClinic
    {
        public bool IsPatientFavorite { get; set; }
        public bool Private {
            get => IsPrivate.GetValueOrDefault(false);
            set => IsPrivate = value;
        }

        public bool Active {
            get => IsActive.GetValueOrDefault(true);
            set => IsActive = value;
        }

        public IFormFile LogoImage { get; set; }
        
        public IFormFile ThumbnailImage { get; set; }

        public string Lat { 
            get
            {
                return Latitude.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
            }
            set
            {
                if(double.TryParse((value ?? "0").Replace('.', ','), out double result))
                    Latitude = result;
            }
        }
        
        public string Long { 
            get
            {
                return Longitude.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
            }
            set
            {
                if(double.TryParse((value ?? "0").Replace('.', ','), out double result))
                    Longitude = result;
            }
        }

        public string FullAddress => $"{Name}, {CityName}, {Street}, {Address}";

        public string RegionName { get; set; }
        public int CityID { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string CityName { get; set; }
        public string CityNameEn { get; set; }
        public string CityNameUz { get; set; }
        public ClinicAccount Account { get; set; }
        public City City { get; set; }
        public List<ClinicPhone> Phones { get; set; }
        public List<ClinicWorkDay> WorkDay { get; set; }
        public List<SelectListItem> Cities { get; set; }

        [IgnoreMap]
        public List<ClinicGallery> Gallery { get; set; }

        [IgnoreMap]
        public ServiceClinic ServiceClinic { get; set; }

        /// <summary>
        /// Initialize only the fields which need to be updated
        /// </summary>
        /// <param name="item">Clinic item</param>
        /// <returns>Returns self with updated properties</returns>
        public Clinic InitUpdate(Clinic item)
        {
            Name = item.Name;
            NameUz = item.NameUz;
            NameEn = item.NameEn;
            Description = item.Description;
            DescriptionUz = item.DescriptionUz;
            DescriptionEn = item.DescriptionEn;
            DateUpdated = DateTime.UtcNow;
            Website = item.Website;
            RegionID = item.RegionID;
            Address = item.Address;
            AddressUz = item.AddressUz;
            AddressEn = item.AddressEn;
            Street = item.Street;
            StreetUz = item.StreetUz;
            StreetEn = item.StreetEn;
            Floor = item.Floor;
            NearestSubway = item.NearestSubway;
            NearestSubwayUz = item.NearestSubwayUz;
            NearestSubwayEn = item.NearestSubwayEn;
            NearBy = item.NearBy;
            NearByUz = item.NearByUz;
            NearByEn = item.NearByEn;
            IsPrivate = item.IsPrivate;
            IsFeatured = item.IsFeatured;
            IsActive = item.Account.IsActive;
            District = item.District;
            DistrictUz = item.DistrictUz;
            DistrictEn = item.DistrictEn;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            SortOrder = item.SortOrder;

            if (!string.IsNullOrWhiteSpace(item.LogoUrl))
                LogoUrl = item.LogoUrl;

            if (!string.IsNullOrWhiteSpace(item.ThumbnailUrl))
                ThumbnailUrl = item.ThumbnailUrl;

            return this;
        }
    }
}
