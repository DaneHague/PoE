using System.Text.Json.Serialization;
using PoE.Services.Models.Cosmos;

namespace PoE.Services.Models.PoE.CosmosItemPrice
{
    [Container("ItemPriceContainer")]
    public class CosmosItemPrice : ICosmosEntity
    {
        public string id { get; set; }
        [JsonPropertyName("itemName")]
        public string? ItemName { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("prices")]
        public List<ItemPrice>? Prices { get; set; }
    }

    public class ItemPrice
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("timeRecorded")]
        public DateTime TimeRecorded { get; set; }
    }
}
