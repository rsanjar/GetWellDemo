using Newtonsoft.Json;

namespace GetWell.API.Models;

public class NameValueModel
{
    public NameValueModel(int id, string name)
    {
        ID = id;
        Name = name;
    }

    [JsonProperty("id")]
    public int ID { get; private set; }
     
    [JsonProperty("name")]
    public string Name { get; private set; }
}