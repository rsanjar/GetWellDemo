using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IServiceCategory))]
    [ModelMetadataType(typeof(IServiceCategory))]
    public partial class ServiceCategory : BaseLocalizable<ServiceCategory>, IServiceCategory
    {
        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public IFormFile IconImage { get; set; }

        public string CategoryName { get; set; }

        public string TitleName { get; set; }
        
        public List<SelectListItem> Categories { get; set; }

        public List<SelectListItem> Titles { get; set; }

        public ServiceCategory InitUpdate(ServiceCategory item)
        {
            Name = item.Name;
            NameUz = item.NameUz;
            NameEn = item.NameEn;
            Description = item.Description;
            DescriptionUz = item.DescriptionUz;
            DescriptionEn = item.DescriptionEn;
            IsActive = item.Active;
            CategoryID = item.CategoryID;
            TitleID = item.TitleID;
            HexColor = item.HexColor;
            IconUrl = item.IconUrl;

            return this;
        }
    }
}