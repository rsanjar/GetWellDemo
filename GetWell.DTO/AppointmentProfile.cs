using System;
using System.Collections.Generic;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO
{
    public partial class AppointmentProfile
    {
        public int ID { get; set; }
        public int AppointmentID { get; set; }
        public string PatientComments { get; set; }
        public string DoctorComments { get; set; }
        public bool IsResolved { get; set; }
        public string AttachmentImageUrl { get; set; }
        public string AttachmentDocUrl { get; set; }
        public string AttachmentPdfUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? PatientRating { get; set; }
        public string PatientReview { get; set; }
        public string QrCodeBase64 { get; set; }
        public DateTime? QrScannedDate { get; set; }
        public int? ClinicDiscountID { get; set; }
    }
}
