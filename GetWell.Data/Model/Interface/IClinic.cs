using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Data.Model.Interface
{
	public interface IClinic : IDateLoggable, IBaseModel
	{
		string Name { get; set; }
		string Website { get; set; }
		string District { get; set; }
		string Street { get; set; }
		string Address { get; set; }
		string NearBy { get; set; }
		double Latitude { get; set; }
		double Longitude { get; set; }
		int RegionID { get; set; }
		int? Floor { get; set; }
		string NearestSubway { get; set; }
		bool? IsPrivate { get; set; }
		int? ParentDepartmentID { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
		string DistrictUz { get; set; }
		string StreetUz { get; set; }
		string AddressUz { get; set; }
		string NearByUz { get; set; }
		string NearestSubwayUz { get; set; }
		string DistrictEn { get; set; }
		string StreetEn { get; set; }
		string AddressEn { get; set; }
		string NearByEn { get; set; }
		string NearestSubwayEn { get; set; }
		string Description { get; set; }
		string DescriptionUz { get; set; }
		string DescriptionEn { get; set; }
		bool IsFeatured { get; set; }
		string LogoUrl { get; set; }
		string ThumbnailUrl { get; set; }
		Guid UniqueKey { get; set; }
        string QrImageCode { get; set; }
        int DiscountPercentage { get; set; }
        DateTime? BusinessStartDate { get; set; }
        DateTime? BusinessEndDate { get; set; }
        bool? IsActive { get; set; }
		int SortOrder { get; set; }

		[JsonIgnore]
		Clinic ParentDepartment { get; set; }

		Region Region { get; set; }

		[JsonIgnore]
		ClinicAccount ClinicAccount { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicDoctor> ClinicDoctors { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicGallery> ClinicGalleries { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicPhone> ClinicPhones { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicReview> ClinicReviews { get; set; }
		
		[JsonIgnore]
		ICollection<ClinicWorkDay> ClinicWorkDays { get; set; }
		
		[JsonIgnore]
		ICollection<Clinic> InverseParentDepartment { get; set; }
    }
}