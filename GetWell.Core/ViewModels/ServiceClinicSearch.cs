using GetWell.DTO;
using Newtonsoft.Json;

namespace GetWell.Core.ViewModels
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServiceClinicSearch : PaginationModel<ServiceClinic>
    {
        [JsonProperty]
        public int ServiceID { get; set; }

        [JsonProperty]
        public int CityID { get; set; }
    }
}