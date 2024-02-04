using System.Text.Json.Serialization;

namespace PoE.Services.Models.Cosmos.PoE.CosmosStashItems
{
    public class EssenceStashResponse
    {
        [JsonPropertyName("stash")]
        public EssenceStash Stash { get; set; }
    }

    public class EssenceStash
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
        public List<CosmosEssenceItems> Items { get; set; }
    }
    [Container("ItemContainer")]
    public class CosmosEssenceItems : ICosmosEntity
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
