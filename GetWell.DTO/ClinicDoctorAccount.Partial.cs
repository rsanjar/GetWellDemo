using System;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicDoctorAccount))]
    [ModelMetadataType(typeof(IClinicDoctorAccount))]
    public partial class ClinicDoctorAccount : IClinicDoctorAccount
    {
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Initializes only the fields which are allowed to be set during a new record creation
        /// </summary>
        /// <param name="item">ClinicDoctorAccount object</param>
        /// <returns>Returns ClinicDoctorAccount object with fields allowed to be inserted</returns>
        public ClinicDoctorAccount InitSave(ClinicDoctorAccount item)
        {
            ClinicDoctorID = item.ClinicDoctorID;
            MobilePhone = item.MobilePhone;
            Email = item.Email;
            Password = item.Password;
            IsActive = item.IsActive;
            DateCreated = DateTime.UtcNow;
            UniqueKey = Guid.NewGuid();
            SmsActivationCode = new Random().Next(10000, 99999);

            return this;
        }

        /// <summary>
        /// Initializes only the fields which are allowed to be updated
        /// </summary>
        /// <param name="item">ClinicDoctorAccount object</param>
        /// <returns>Returns ClinicDoctorAccount object with fields allowed to be updated</returns>
        public ClinicDoctorAccount InitUpdate(ClinicDoctorAccount item)
        {
            MobilePhone = item.MobilePhone;
            Email = item.Email;
            IsActive = item.IsActive;
            
            return this;
        }
    }
}
