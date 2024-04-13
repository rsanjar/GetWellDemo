using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public interface IRootResponse<TResult> : IResponseBase where TResult : IResult
{
    [JsonProperty("result")]
    public TResult Result { get; set; }
}