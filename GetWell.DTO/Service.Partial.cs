using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IService))]
    [ModelMetadataType(typeof(IService))]
    public partial class Service : BaseLocalizable<Service>, IService
    {
        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public IFormFile IconImage { get; set; }

        public int CategoryID { get; set; }

        public string ServiceCategoryName { get; set; }

        public List<SelectListItem> ServiceCategories { get; set; }

        public Service InitUpdate(Service item)
        {
            Name = item.Name;
            NameUz = item.NameUz;
            NameEn = item.NameEn;
            Description = item.Description;
            DescriptionUz = item.DescriptionUz;
            DescriptionEn = item.DescriptionEn;
            IsActive = item.Active;
            ServiceCategoryID = item.ServiceCategoryID;
            SortOrder = item.SortOrder;
            IconUrl = item.IconUrl;

            return this;
        }
    }
}
