using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request;

[JsonObject(MemberSerialization.OptIn)]
public interface IRestRequest<TRequest> where TRequest : IBaseRequest<IResponseBase>
{
    [JsonProperty("id")]
    public int TransactionID { get; set; }

    [JsonProperty("method")]
    public string PaymentMethodName { get; init; }

    [JsonProperty("params")]
    public TRequest RequestObject { get; init; }
}