using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

[JsonObject(MemberSerialization.OptIn)]
public class RootResponse<TResult> : IRootResponse<TResult> where TResult : IResult
{
    public string Jsonrpc { get; set; }
    
    public int ID { get; set; }
    
    public TResult Result { get; set; }
}