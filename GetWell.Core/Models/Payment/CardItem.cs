using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment;

public class CardItem : CardBaseItem
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("recurrent")]
    public bool Recurrent { get; set; }

    [JsonProperty("verify")]
    public bool Verify { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}