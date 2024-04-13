using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
	[MetadataType(typeof(ITitle))]
	[ModelMetadataType(typeof(ITitle))]
    public partial class Title : BaseLocalizable<Title>, ITitle
    {
        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public Title InitUpdate(Title item)
        {
            Name = item.Name;
            NameEn = item.NameEn;
            NameUz = item.NameUz;
            IsActive = item.IsActive;
            SortOrder = item.SortOrder;
            
            return this;
        }
    }
}
