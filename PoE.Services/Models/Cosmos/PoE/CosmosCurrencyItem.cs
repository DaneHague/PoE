using System.Text.Json.Serialization;

namespace PoE.Services.Models.Cosmos.PoE
{
    public class StashResponse
    {
        [JsonPropertyName("stash")]
        public CurrencyStash Stash { get; set; }
    }

    public class CurrencyStash
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("items")]
        public List<CosmosCurrencyItem> Items { get; set; }
    }
    [Container("ItemContainer")]
    public class CosmosCurrencyItem : ICosmosEntity
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("typeLine")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("stackSize")]
        public int StackSize { get; set; }

        [JsonPropertyName("maxStackSize")]
        public int MaxStackSize { get; set; }
    }
}

