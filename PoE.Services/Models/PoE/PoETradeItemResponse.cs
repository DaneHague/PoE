namespace PoE.Services.Models.PoE;

using System.Text.Json.Serialization;

public class PoETradeItemResponse
{
    [JsonPropertyName("result")]
    public List<Result>? Result { get; set; }
}

public class Result
{
    [JsonPropertyName("listing")]
    public Listing Listing { get; set; }
}

public class Listing
{
    [JsonPropertyName("price")]
    public Price Price { get; set; }
}

public class Price
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}
