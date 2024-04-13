using System;
using System.Text.Json.Serialization;

namespace GetWell.Data.Model.Interface
{
    public interface IServiceClinicDoctor : IDateLoggable, IBaseModel
    {
        int ServiceClinicID { get; set; }
        int ClinicDoctorID { get; set; }
        bool? IsActive { get; set; }
        TimeSpan? AverageDuration { get; set; }
        decimal? Price { get; set; }
        string Description { get; set; }
        string DescriptionUz { get; set; }
        string DescriptionEn { get; set; }

        [JsonIgnore]
        ClinicDoctor ClinicDoctor { get; set; }

        [JsonIgnore]
        ServiceClinic ServiceClinic { get; set; }
    }
}