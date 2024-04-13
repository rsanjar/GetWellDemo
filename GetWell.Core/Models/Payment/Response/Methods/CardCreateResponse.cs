using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response.Methods;

public class CardCreateResponse : IResult
{
    [JsonProperty("card")]
    public CardItem Card { get; set; }
}