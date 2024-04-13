using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request;

public interface IBaseRequest<TResponse> where TResponse : IResponseBase
{
    [JsonIgnore]
    public string MethodName { get; }
}