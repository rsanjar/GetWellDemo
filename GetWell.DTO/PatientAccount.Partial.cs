using System;
using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.DTO
{
    [MetadataType(typeof(IAccount))]
    [ModelMetadataType(typeof(IAccount))]
    public partial class PatientAccount : IPatientAccount
    {
        /// <summary>
        /// Initializes only the fields which are allowed to be set during a new record creation
        /// </summary>
        /// <param name="item">PatientAccount object</param>
        /// <returns>Returns PatientAccount object with fields allowed to be inserted</returns>
        public PatientAccount InitSave(PatientAccount item)
        {
            PatientID = item.PatientID;
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
        /// <param name="item">PatientAccount object</param>
        /// <returns>Returns PatientAccount object with fields allowed to be updated</returns>
        public PatientAccount InitUpdate(PatientAccount item)
        {
            Email = item.Email;
            IsActive = item.IsActive;
            
            return this;
        }
    }
}
