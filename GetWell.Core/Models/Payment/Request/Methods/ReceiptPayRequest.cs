using GetWell.Core.Enums;
using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request.Methods;

public class ReceiptPayRequest : CardTokenItem, IBaseRequest<IResponseBase>
{
    public string MethodName => PaymentMethods.ReceiptPay;

    [JsonProperty("id")]
    public string ID { get; set; }

    [JsonProperty("Payer")]
    public Payer Payer { get; set; }
}