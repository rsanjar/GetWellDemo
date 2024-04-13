using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Doctor
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? RetirementDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsRetired { get; set; }
        public bool IsFamilyDoctor { get; set; }
        public DateTime? CareerStartDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string FirstNameUz { get; set; }
        public string LastNameUz { get; set; }
        public string MiddleNameUz { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public string MiddleNameEn { get; set; }
    }
}
