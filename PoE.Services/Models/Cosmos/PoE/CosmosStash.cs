using System.Text.Json.Serialization;
using PoE.Services.Models.Cosmos;

namespace PoE.Services.Models;

public class Stash
{
    [JsonPropertyName("stashes")]
    public List<CosmosStash> Stashes { get; set; }
    
}

[Container("StashContainer")]
public class CosmosStash : ICosmosEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }

    public string id { get; set; }
}