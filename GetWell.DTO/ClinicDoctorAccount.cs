using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class ClinicDoctorAccount
    {
        public int ID { get; set; }
        public int ClinicDoctorID { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public Guid UniqueKey { get; set; }
        public int? SmsActivationCode { get; set; }
        public DateTime? SmsActivationDate { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? EmailVerificationDate { get; set; }
        public DateTime? LastVerificationAttemptDate { get; set; }
    }
}
