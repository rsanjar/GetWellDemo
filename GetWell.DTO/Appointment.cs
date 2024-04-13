using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class Appointment
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsRefunded { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime? AppointmentEndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid ConfirmationCode { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public bool? IsDoctorConfirmed { get; set; }
        public int ServiceClinicDoctorID { get; set; }
        public bool IsArchived { get; set; }
        public decimal? PriceBeforeDiscount { get; set; }
        public bool IsPaid { get; set; }
    }
}
