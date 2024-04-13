using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Clinic
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string NearBy { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RegionID { get; set; }
        public int? Floor { get; set; }
        public string NearestSubway { get; set; }
        public bool? IsPrivate { get; set; }
        public int? ParentDepartmentID { get; set; }
        public string NameUz { get; set; }
        public string NameEn { get; set; }
        public string DistrictUz { get; set; }
        public string StreetUz { get; set; }
        public string AddressUz { get; set; }
        public string NearByUz { get; set; }
        public string NearestSubwayUz { get; set; }
        public string DistrictEn { get; set; }
        public string StreetEn { get; set; }
        public string AddressEn { get; set; }
        public string NearByEn { get; set; }
        public string NearestSubwayEn { get; set; }
        public string DescriptionUz { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsFeatured { get; set; }
        public string LogoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public Guid UniqueKey { get; set; }
        public string QrImageCode { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime? BusinessStartDate { get; set; }
        public DateTime? BusinessEndDate { get; set; }
        public bool? IsActive { get; set; }
        public int SortOrder { get; set; }
    }
}
