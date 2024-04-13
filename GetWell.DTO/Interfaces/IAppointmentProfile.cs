using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces;

public interface IAppointmentProfile : IBaseModel
{
	[Required]
	public int AppointmentID { get; set; }

	[Display(Name = "Комментарий Пациента", Prompt = "Комментарий Пациента")]
	public string PatientComments { get; set; }
	
	[Display(Name = "Комментарий Врача", Prompt = "Комментарий Врача")]
	public string DoctorComments { get; set; }

	[Display(Name = "Закрыт", Prompt = "Закрыт")]
	public bool IsResolved { get; set; }
	
	[Display(Name = "Ссылка на Фото", Prompt = "Ссылка на Фото")]
	public string AttachmentImageUrl { get; set; }

	[Display(Name = "Ссылка на Документ", Prompt = "Ссылка на Документ")]
	public string AttachmentDocUrl { get; set; }
	
	[Display(Name = "Ссылка на PDF Документ", Prompt = "Ссылка на PDF Документ")]
	public string AttachmentPdfUrl { get; set; }
	
	public DateTime DateCreated { get; set; }
	
	public DateTime? DateUpdated { get; set; }
	
	[Display(Name = "Рейтинг", Prompt = "Рейтинг")]
	public int? PatientRating { get; set; }
	
	[Display(Name = "Отзыв", Prompt = "Отзыв")]
	public string PatientReview { get; set; }	

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public string QrCodeBase64 { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public DateTime? QrScannedDate { get; set; }

    public int? ClinicDiscountID { get; set; }

    public int ServiceClinicID { get; set; }

    public int ServiceID { get; set; }

    public int ClinicID { get; set; }
        
    public int ClinicDoctorID { get; set; }
        
    public string ClinicName { get; set; }

    public string ServiceName { get; set; }
        
    public string DoctorFullName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string DoctorFullNameUz { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string DoctorFullNameEn { get; set; }

        
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string ServiceNameUz { get; set; }
        
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string ServiceNameEn { get; set; }
        
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string ClinicNameUz { get; set; }
        
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string ClinicNameEn { get; set; }

    string PatientFullName { get; set; }
}