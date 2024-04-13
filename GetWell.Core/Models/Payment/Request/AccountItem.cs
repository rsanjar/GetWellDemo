using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Request;

public class AccountItem
{
    [JsonProperty("order_id")]
    public int OrderID { get; set; }
}