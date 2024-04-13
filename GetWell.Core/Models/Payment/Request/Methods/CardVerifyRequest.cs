using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class CardVerifyRequest : CardTokenItem, IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.CardVerify;

    [JsonProperty("code")]
    public string Code { get; set; }
}