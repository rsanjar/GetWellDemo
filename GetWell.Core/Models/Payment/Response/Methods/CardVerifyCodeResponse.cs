using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response.Methods;

public class CardVerifyCodeResponse : IResult
{
    [JsonProperty("sent")]
    public bool Sent { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("wait")]
    public int Wait { get; set; }
}