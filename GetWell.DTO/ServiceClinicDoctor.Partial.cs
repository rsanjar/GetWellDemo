using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IServiceClinicDoctor))]
    [ModelMetadataType(typeof(IServiceClinicDoctor))]
    public partial class ServiceClinicDoctor : BaseLocalizable<ServiceClinicDoctor>, IServiceClinicDoctor
    {
        public int ClinicID { get; set; }

        public int? ServiceID { get; set; }

        public int? DoctorID { get; set; }

        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }

        public int? Duration
        {
            get => AverageDuration.GetValueOrDefault(TimeSpan.Zero).TotalMinutes == 0 ? null : (int)AverageDuration.GetValueOrDefault(TimeSpan.Zero).TotalMinutes;
            set => AverageDuration = TimeSpan.FromMinutes(value.GetValueOrDefault(0));
        }

        public int? PriceInt
        {
            get => Price != null ? (int)Price : null;
            set => Price = value;
        }
        
        [IgnoreMap]
        public Service Service { get; set; }

        [IgnoreMap]
        public List<Appointment> Appointments { get; set; } = new();
    }
}