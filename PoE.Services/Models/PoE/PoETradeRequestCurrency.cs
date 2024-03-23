using System.Text.Json.Serialization;

namespace PoE.Services.Models.PoE.Currency
{
    public class PoETradeRequestCurrency
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
    
        [JsonPropertyName("term")]
        public string Term { get; set; }
    
        [JsonPropertyName("filters")]
        public Filters Filters { get; set; }
    }
    
    public class Filters
    {
        [JsonPropertyName("trade_filters")]
        public TradeFilters TradeFilters { get; set; }
    }
    
    public class TradeFilters
    {
        [JsonPropertyName("filters")]
        public TradeFilterDetails Filters { get; set; }
    }
    
    public class TradeFilterDetails
    {
        [JsonPropertyName("category")]
        public Category Category { get; set; }
    }
    
    public class Category
    {
        [JsonPropertyName("option")]
        public string Option { get; set; }
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
        public List<object> Filters { get; set; }
    }
    
    public class Sort
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}

