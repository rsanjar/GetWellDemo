using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response.Methods;

public class ReceiptPayResponse : IResult
{
    [JsonProperty("receipt")]
    public Receipt Receipt { get; set; }
}