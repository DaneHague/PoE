using System.Text.Json.Serialization;
using PoE.Services.Models.Cosmos;

namespace PoE.Services.Models.PoE;

[Container("ItemPriceContainer")]
public class CosmosItem: ICosmosEntity
{
    public string id { get; set; }
    [JsonPropertyName("itemName")]
    public string? ItemName { get; set; }
}