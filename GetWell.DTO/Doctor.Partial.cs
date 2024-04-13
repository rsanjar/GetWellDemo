using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IDoctor))]
    [ModelMetadataType(typeof(IDoctor))]
    public partial class Doctor : BaseLocalizable<Doctor>, IDoctor
    {
        public bool Active
        {
            get => IsActive.GetValueOrDefault(false);
            set => IsActive = value;
        }
        public int? ClinicID { get; set; }
        public string ClinicName { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public ClinicDoctorAccount Account { get; set; }
        public DoctorProfile Profile { get; set; }
        public List<DoctorPhone> Phones { get; set; }
        public List<ClinicDoctorWorkDay> WorkDay { get; set; }

        public Doctor InitUpdate(Doctor item)
        {
            FirstName = item.FirstName;
            LastName = item.LastName;
            MiddleName = item.MiddleName;
            FirstNameUz = item.FirstNameUz;
            LastNameUz = item.LastNameUz;
            MiddleNameUz = item.MiddleNameUz;
            FirstNameEn = item.FirstNameEn;
            LastNameEn = item.LastNameEn;
            MiddleNameEn = item.MiddleNameEn;
            Email = item.Email;
            DateOfBirth = item.DateOfBirth;
            RetirementDate = item.RetirementDate;
            IsActive = item.IsActive;
            IsRetired = item.IsRetired;
            IsFamilyDoctor = item.IsFamilyDoctor;
            CareerStartDate = item.CareerStartDate;
            DateUpdated = DateTime.UtcNow;

            return this;
        }
    }
}
