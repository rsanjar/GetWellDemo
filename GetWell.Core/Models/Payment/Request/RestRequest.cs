using GetWell.Core.Models.Payment.Response;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
using System.Threading;

namespace GetWell.Core.Models.Payment.Request;

public sealed class RestRequest<TRequest, TResult> : IRestRequest<TRequest>
    where TResult : IResponseBase
    where TRequest : IBaseRequest<IResponseBase>
{
    public RestRequest(RestClient restClient, TRequest request)
    {
        RequestObject = request;
        PaymentMethodName = request.MethodName;
        RestClientItem = restClient;
    }
    
    public int TransactionID { get; set; }

    public string PaymentMethodName { get; init; }

    public TRequest RequestObject { get; init; }
    
    public RestClient RestClientItem { get; init; }
    
    public async Task<TResult> RequestAsync(Method method = Method.Post,
        CancellationToken cancellationToken = default, params HeaderParameter[] headerParameters)
    {
        var request = new RestRequest();
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Cache-Control", "no-cache");

        foreach (var parameter in headerParameters)
            request.AddHeader(parameter.Name, parameter.Value?.ToString());

        var body = JsonConvert.SerializeObject(this, Formatting.Indented);
        
        request.AddJsonBody(body);

        var json = await RestClientItem.ExecuteAsync(request, method, cancellationToken);
        var response = JsonConvert.DeserializeObject<TResult>(json.Content);
        
        return response;
    }
}