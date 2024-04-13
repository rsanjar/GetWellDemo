using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(IClinicDiscount))]
	[ModelMetadataType(typeof(IClinicDiscount))]
    public partial class ClinicDiscount : BaseLocalizable<ClinicDiscount>, IClinicDiscount
    {
        public int DiscountPercentageInt
        {
            get => (int)DiscountPercentage;
            set => DiscountPercentage = value;
        }

        public string PriceBeforeDiscountInt
        {
            get => ((int)PriceBeforeDiscount).ToString();
            set => PriceBeforeDiscount = int.Parse(value.Replace(" ", ""));
        }

        public decimal PriceAfterDiscount => PriceBeforeDiscount - (PriceBeforeDiscount * DiscountPercentage) / 100;

        public int ClinicID { get; set; }
	    public IFormFile DiscountImage { get; set; }
	    public string ClinicName { get; set; }
        public string ClinicNameEn { get; set; }
	    public string ClinicNameUz { get; set; }
	    public string ClinicLogoUrl { get; set; }
	    public int ClinicCityID { get; set; }
	    public int ClinicCityRegionID { get; set; }
		public string ServiceName { get; set; }
		public string ServiceNameUz { get; set; }
		public string ServiceNameEn { get; set; }
		public string ServiceCategoryName { get; set; }
		public string ServiceCategoryNameUz { get; set; }
		public string ServiceCategoryNameEn { get; set; }

		[IgnoreMap]
		public ServiceClinic ServiceClinic { get; set; }


        public ClinicDiscount InitUpdate(ClinicDiscount item)
        {
            Title = item.Title;
            Body = item.Body;
            TitleUz = item.TitleUz;
            TitleEn = item.TitleEn;
            BodyUz = item.BodyUz;
            BodyEn = item.BodyEn;
            DiscountPercentage = item.DiscountPercentage;
            PriceBeforeDiscount = item.PriceBeforeDiscount;
            StartDate = item.StartDate;
            EndDate = item.EndDate;
            IsActive = item.IsActive;
            DateUpdated = DateTime.UtcNow;
            ServiceClinicID = item.ServiceClinicID;
            SortOrder = item.SortOrder;

            if (!string.IsNullOrWhiteSpace(item.ImageUrl))
                ImageUrl = item.ImageUrl;
		
            return this;
        }


    }
}
