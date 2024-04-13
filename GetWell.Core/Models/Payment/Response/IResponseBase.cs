using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public interface IResponseBase
{
    [JsonProperty("jsonrpc")]
    public string Jsonrpc { get; set; }

    [JsonProperty("id")]
    public int ID { get; set; }
}