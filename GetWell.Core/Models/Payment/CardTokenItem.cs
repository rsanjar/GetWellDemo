using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment;

public abstract class CardTokenItem
{
    [JsonProperty("token")]
    public string Token { get; set; }
}