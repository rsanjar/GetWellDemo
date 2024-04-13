using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public class Meta
{
    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("owner")]
    public string Owner { get; set; }

    [JsonProperty("host")]
    public string Host { get; set; }
}