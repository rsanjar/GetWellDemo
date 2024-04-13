using System.ComponentModel.DataAnnotations;
using GetWell.DTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GetWell.DTO
{
    [MetadataType(typeof(IClinicAccount))]
    [ModelMetadataType(typeof(IClinicAccount))]
    public partial class ClinicAccount : IClinicAccount
    {
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Initializes only the fields which are allowed to be set during a new record creation
        /// </summary>
        /// <param name="item">ClinicAccount object</param>
        /// <returns>Returns ClinicAccount object with fields allowed to be inserted</returns>
        public ClinicAccount InitSave(ClinicAccount item)
        {
            ClinicID = item.ClinicID;
            MobilePhone = item.MobilePhone;
            Email = item.Email;
            Password = item.Password;
            IsActive = item.IsActive;

            return this;
        }

        /// <summary>
        /// Initializes only the fields which are allowed to be updated
        /// </summary>
        /// <param name="item">ClinicAccount object</param>
        /// <returns>Returns ClinicAccount object with fields allowed to be updated</returns>
        public ClinicAccount InitUpdate(ClinicAccount item)
        {
            MobilePhone = item.MobilePhone;
            Email = item.Email;
            IsActive = item.IsActive;

            return this;
        }
    }
}
