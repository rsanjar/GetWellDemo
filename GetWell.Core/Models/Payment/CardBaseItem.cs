using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment;

public class CardBaseItem
{
    [JsonProperty("number")]
    [StringLength(16, MinimumLength = 16)]
    [DataType(DataType.CreditCard)]
    public string Number { get; set; }

    [JsonProperty("expire")]
    [StringLength(4, MinimumLength = 4)]
    public string Expire { get; set; }
}