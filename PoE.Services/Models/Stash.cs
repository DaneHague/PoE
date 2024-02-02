using System.Text.Json.Serialization;

namespace PoE.Services.Models;

public class Stash
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }
}