using System.Text.Json.Serialization;

namespace PoE.Services.Models.PoE;

public class PoETradeRequestResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("complexity")]
    public int Complexity { get; set; }
    [JsonPropertyName("result")]
    public List<string> Result { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}