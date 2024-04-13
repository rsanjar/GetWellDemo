using GetWell.DTO;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class ReviewSearch : PaginationModel<AppointmentProfile>
{
    [JsonProperty]
    public int? ClinicID { get; set; }

    [JsonProperty]
    public int? ClinicDoctorID { get; set; }

    [JsonProperty]
    public int? ServiceClinicID { get; set; }

    [JsonProperty]
    public int? ServiceClinicDoctorID { get; set; }
}