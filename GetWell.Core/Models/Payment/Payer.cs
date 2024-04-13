using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment;

public class Payer
{
    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("ip")]
    public string IP { get; set; }
}