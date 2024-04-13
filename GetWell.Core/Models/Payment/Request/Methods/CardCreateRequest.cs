using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class CardCreateRequest : IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.CardCreate;

    [JsonProperty("card")]
    public CardBaseItem Card { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }

    [JsonProperty("save")]
    public bool Save { get; } = true;
}