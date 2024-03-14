using System.Text.Json.Serialization;

namespace PoE.Services.Models.PoE.PoETradeRequest
{
    public class PoETradeRequest
    {
        [JsonPropertyName("query")]
        public Query Query { get; set; }

        [JsonPropertyName("sort")]
        public Sort Sort { get; set; }
    }

    public class Query
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("stats")]
        public List<Stat> Stats { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("option")]
        public string Option { get; set; }
    }

    public class Stat
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("filters")]
        public List<object> Filters { get; set; } // Assuming filters is a list but unsure of the type
    }

    public class Sort
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}

