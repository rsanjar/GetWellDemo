using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class ReceiptCreateRequest : CardTokenItem, IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.ReceiptCreate;

    [JsonProperty("account")]
    public AccountItem Account { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }

    [JsonProperty("hold")]
    public bool Hold { get; set; }
}