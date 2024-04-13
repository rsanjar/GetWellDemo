using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public class Merchant
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("organization")]
    public string Organization { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("business_id")]
    public string BusinessId { get; set; }

    [JsonProperty("epos")]
    public Epos Epos { get; set; }

    [JsonProperty("date")]
    public long Date { get; set; }

    [JsonProperty("logo")]
    public object Logo { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("terms")]
    public object Terms { get; set; }
}