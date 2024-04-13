using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GetWell.DTO
{
    [MetadataType(typeof(IAppointment))]
    [ModelMetadataType(typeof(IAppointment))]
    public partial class Appointment : BaseLocalizable<Appointment>, IAppointment
    {
        public int? ClinicID { get; set; }

        public int? DoctorID { get; set; }

        public string DoctorProfilePictureUrl { get; set; }

        public TimeSpan AverageDuration { get; set; }

        public TimeSpan EndTime => AppointmentTime + AverageDuration;

        public string ClinicName => Clinic?.Name;

        public string DoctorName => Doctor?.FullName;
        
        public string PatientName { get; set; }

        public string ServiceName { get; set; }

        public string AppointmentTimeStr { get; set; }

        public bool DoctorConfirmed
        {
            get => IsDoctorConfirmed.GetValueOrDefault(false);
            set => IsDoctorConfirmed = value;
        }

        public int ServiceCategoryID { get; set; }

        public int ServiceClinicID { get; set; }

        public int ClinicDoctorID { get; set; }
        
        [IgnoreMap]
        public Patient Patient { get; set; }

        [IgnoreMap]
        public PatientProfile PatientProfile { get; set; }

        [IgnoreMap]
        public Doctor Doctor { get; set; }
        
        [IgnoreMap]
        public Clinic Clinic { get; set; }
        
        [IgnoreMap]
        public AppointmentProfile AppointmentProfile { get; set; }

        [IgnoreMap]
        public ServiceClinicDoctor ServiceClinicDoctor { get; set; }

        public List<SelectListItem> ServiceCategories { get; set; } = new();

        public List<SelectListItem> Services { get; set; } = new();

        public List<SelectListItem> Doctors { get; set; } = new();

        public Appointment InitUpdate(Appointment model)
        {
            AppointmentDate = model.AppointmentDate;
            AppointmentTime = model.AppointmentTime;
            IsCanceled = model.IsCanceled;
            IsRefunded = model.IsRefunded;
            DiscountPercentage = model.DiscountPercentage;
            IsDoctorConfirmed = model.IsDoctorConfirmed;
            IsArchived = model.IsArchived;
            IsPaid = model.IsPaid;
            
            if(IsDoctorConfirmed.GetValueOrDefault(false))
                ConfirmationDate = DateTime.UtcNow;
            
            return this;
        }

        public Appointment InitSave(Appointment model)
        {
            PatientID = model.Patient.ID;
            AppointmentDate = model.AppointmentDate;
            AppointmentTime = model.AppointmentTime;
            DiscountPercentage = 0;
            ConfirmationCode = Guid.NewGuid();
            IsArchived = false;
            IsPaid = model.IsPaid;
            IsCanceled = false;
            IsRefunded = false;
            IsDoctorConfirmed = model.DoctorConfirmed;

            return this;
        }
    }
}
