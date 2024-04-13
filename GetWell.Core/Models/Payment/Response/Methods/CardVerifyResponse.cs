using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response.Methods;

public class CardVerifyResponse : IResult
{
    [JsonProperty("card")]
    public CardItem Card { get; set; }
}