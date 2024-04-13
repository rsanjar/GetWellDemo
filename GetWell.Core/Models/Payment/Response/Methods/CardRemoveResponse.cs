using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response.Methods;

public class CardRemoveResponse : IResult
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}