namespace PoE.Services;

public interface ITimerTriggerService
{
    Task<List<(decimal MeanPrice, string Currency)>> FetchTradeResponsesAndCalculateMeanPrice(string itemName,
        int maxResponses, bool isCurrency = false);
    Task UpsertItemPrice(string itemName, List<(decimal Price, string Currency)> prices);
}