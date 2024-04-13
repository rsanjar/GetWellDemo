using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GetWell.DTO.Interfaces;
using GetWell.DTO.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GetWell.DTO
{
    [MetadataType(typeof(IPatient))]
    [ModelMetadataType(typeof(IPatient))]
    public partial class Patient : BaseLocalizable<Patient>, IPatient
    {
        public int? ClinicID { get; set; }

        public string ClinicName { get; set; }

        public string CityName { get; set; }
        public string CityNameUz { get; set; }
        public string CityNameEn { get; set; }

        public int? DoctorID { get; set; }

        public string FullName => $"{FirstName} {LastName} {MiddleName}";

        public List<SelectListItem> Languages { get; set; }
        
        [IgnoreMap]
        public PatientAccount PatientAccount { get; set; }
        
        [IgnoreMap]
        public PatientProfile PatientProfile { get; set; }

        public Patient InitUpdate(Patient model)
        {
            ID = model.ID;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Phone = model.Phone;
            MiddleName = model.MiddleName;
            SecondaryPhone = model.SecondaryPhone;
            Email = model.Email;
            IsActive = model.IsActive;
            PreferredLanguage = model.PreferredLanguage;
            CityID = model.CityID;

            return this;
        }
    }
}
