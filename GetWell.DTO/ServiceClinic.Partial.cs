using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IServiceClinic))]
    [ModelMetadataType(typeof(IServiceClinic))]
    public partial class ServiceClinic : BaseLocalizable<ServiceClinic>, IServiceClinic
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string NameUz { get; set; }

        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public int? Duration
        {
            get => AverageDuration.TotalMinutes == 0 ? null : (int)AverageDuration.TotalMinutes;
            set => AverageDuration = TimeSpan.FromMinutes(value.GetValueOrDefault(0));
        }

        public int? PriceInt
        {
            get => Price != null ? (int)Price : null;
            set => Price = value;
        }

        public string PriceString
        {
            get => Price != null ? ((int)Price).ToString() : null;
            set => Price = int.Parse((value ?? "0").Replace(" ", ""));
        }
        
        public Service Service { get; set; }

        [IgnoreMap]
        public Clinic Clinic { get; set; }
    }
}