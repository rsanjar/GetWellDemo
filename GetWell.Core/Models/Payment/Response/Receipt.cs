using System.Collections.Generic;
using Newtonsoft.Json;

namespace GetWell.Core.Models.Payment.Response;

public class Receipt
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("create_time")]
    public long CreateTime { get; set; }

    [JsonProperty("pay_time")]
    public long PayTime { get; set; }

    [JsonProperty("cancel_time")]
    public long CancelTime { get; set; }

    [JsonProperty("state")]
    public int State { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("external")]
    public bool External { get; set; }

    [JsonProperty("operation")]
    public int Operation { get; set; }

    [JsonProperty("category")]
    public object Category { get; set; }

    [JsonProperty("error")]
    public object Error { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("detail")]
    public object Detail { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }

    [JsonProperty("currency")]
    public int Currency { get; set; }

    [JsonProperty("commission")]
    public int Commission { get; set; }

    [JsonProperty("account")]
    public List<Account> Account { get; set; }

    [JsonProperty("card")]
    public CardBaseItem Card { get; set; }

    [JsonProperty("payer")]
    public Payer Payer { get; set; }

    [JsonProperty("merchant")]
    public Merchant Merchant { get; set; }

    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("processing_id")]
    public object ProcessingId { get; set; }
}