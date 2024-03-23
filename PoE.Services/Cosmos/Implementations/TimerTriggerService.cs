using PoE.Services.Models.PoE.CosmosItemPrice;

namespace PoE.Services.Implementations;

public class TimerTriggerService : ITimerTriggerService
{
    private readonly ICosmosService _cosmosService;
    private readonly IGetTradeRequestResponseService _getTradeRequestResponseService;

    public TimerTriggerService(
        ICosmosService cosmosService,
        IGetTradeRequestResponseService getTradeRequestResponseService)
    {
        _cosmosService = cosmosService;
        _getTradeRequestResponseService = getTradeRequestResponseService;
    }

    public async Task<List<(decimal MeanPrice, string Currency)>> FetchTradeResponsesAndCalculateMeanPrice(string itemName, int maxResponses)
    {
        var tradeRequestResponses = await _getTradeRequestResponseService.GetTradeRequestResponse(itemName);

        List<(decimal Price, string Currency)> prices = new List<(decimal Price, string Currency)>();

        if (tradeRequestResponses != null)
        {
            int loopLength = tradeRequestResponses.Result.Count >= maxResponses ? maxResponses : tradeRequestResponses.Result.Count;

            for (int i = 1; i < loopLength; i++)
            {
                var tradeItemResponse = await _getTradeRequestResponseService.GetTradeItemResponse(tradeRequestResponses.Result[i]);
                if (tradeItemResponse != null)
                {
                    prices.Add((tradeItemResponse.Result[0].Listing.Price.Amount, tradeItemResponse.Result[0].Listing.Price.Currency));
                }
            }
        }

        var meanPrices = prices
            .GroupBy(p => p.Currency)
            .Select(g => (MeanPrice: g.Average(p => p.Price), Currency: g.Key))
            .ToList();

        return meanPrices;
    }

    public async Task UpsertItemPrice(string itemName, List<(decimal Price, string Currency)> prices)
    {
        var existingItems = await _cosmosService.GetAllItemsAsync<CosmosItemPrice>();
        var existingItem = existingItems.FirstOrDefault(i => i.ItemName.Equals(itemName));

        CosmosItemPrice cosmosItemPrice;

        if (existingItem != null)
        {
            cosmosItemPrice = existingItem;
            foreach (var price in prices)
            {
                cosmosItemPrice.Prices.Add(new ItemPrice
                {
                    Price = price.Price,
                    TimeRecorded = DateTime.Now,
                    Currency = price.Currency
                });
            }
        }
        else
        {
            cosmosItemPrice = new CosmosItemPrice
            {
                id = Guid.NewGuid().ToString(),
                ItemName = itemName,
                Type = "Item",
                Prices = prices.Select(price => new ItemPrice
                {
                    Price = price.Price,
                    TimeRecorded = DateTime.UtcNow,
                    Currency = price.Currency
                }).ToList()
            };
        }

        await _cosmosService.UpsertItemAsync(cosmosItemPrice, cosmosItemPrice.ItemName);
    }
}