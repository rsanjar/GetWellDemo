using System;

namespace GetWell.Data.Model.Interface
{
	public interface IAppointmentProfile : IDateLoggable, IBaseModel
	{
		int AppointmentID { get; set; }
		string PatientComments { get; set; }
		string DoctorComments { get; set; }
		bool IsResolved { get; set; }
		string AttachmentImageUrl { get; set; }
		string AttachmentDocUrl { get; set; }
		string AttachmentPdfUrl { get; set; }
		int? PatientRating { get; set; }
		string PatientReview { get; set; }
		string QrCodeBase64 { get; set; }
		DateTime? QrScannedDate { get; set; }
		int? ClinicDiscountID { get; set; }
	}
}