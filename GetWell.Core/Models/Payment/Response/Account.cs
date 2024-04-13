using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public class Account
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("main")]
    public bool Main { get; set; }
}