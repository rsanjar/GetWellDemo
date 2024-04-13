using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public class Epos
{
    [JsonProperty("merchantId")]
    public string MerchantId { get; set; }

    [JsonProperty("terminalId")]
    public string TerminalId { get; set; }
}