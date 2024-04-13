using System;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(ICategory))]
    [ModelMetadataType(typeof(ICategory))]
    public partial class Category : BaseLocalizable<Category>, ICategory
    {
	    public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public IFormFile IconImage { get; set; }

        public Category InitSave()
        {
            DateCreated = DateTime.UtcNow;

            return this;
        }
        
        public Category InitUpdate(Category item)
        {
            Name = item.Name;
            NameEn = item.NameEn;
            NameUz = item.NameUz;
            Description = item.Description;
            DescriptionEn = item.DescriptionEn;
            DescriptionUz = item.DescriptionUz;
            IsActive = item.Active;
            SortOrder = item.SortOrder;
            HexColor = item.HexColor;
            DateUpdated = DateTime.UtcNow;
            
            if(!string.IsNullOrWhiteSpace(item.IconUrl))
                IconUrl = item.IconUrl;

            return this;
        }
    }
}
