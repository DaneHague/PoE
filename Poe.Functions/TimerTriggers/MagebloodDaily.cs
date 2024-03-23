using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models.PoE.CosmosItemPrice;

namespace Poe.Functions.Triggers;

public class MagebloodDaily
{
    private readonly ICosmosService _cosmosService;
    private readonly IGetTradeRequestResponseService _getTradeRequestResponseService;

    public MagebloodDaily(
        ICosmosService cosmosService,
        IGetTradeRequestResponseService getTradeRequestResponseService)
    {
        _cosmosService = cosmosService;
        _getTradeRequestResponseService = getTradeRequestResponseService;
    }
    
    [FunctionName("MageBloodDaily")]
    public async Task RunAsync([TimerTrigger("0 0 18 * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Mageblood started at: {DateTime.UtcNow}");
        
        var tradeRequestResponses = await _getTradeRequestResponseService.GetTradeRequestResponse("Mageblood");
        
        if (tradeRequestResponses != null)
        {
            int loopLength = tradeRequestResponses.Result.Count >= 5 ? 5 : tradeRequestResponses.Result.Count;
            List<decimal> prices = new List<decimal>();
            
            // Start at 1 to skip the first item in the list
            // This is because the first item is the most recent and we want to ignore it
            // Could be price manipulation
            for (int i = 1; i < loopLength; i++)
            {
                var tradeItemResponse = await _getTradeRequestResponseService.GetTradeItemResponse(tradeRequestResponses.Result[i]);
                if (tradeItemResponse != null)
                {
                    prices.Add(tradeItemResponse.Result[0].Listing.Price.Amount);
                }
            }
            
            decimal mean = prices.Sum() / prices.Count;
            
            string itemName = "MageBlood";
            var existingItems = await _cosmosService.GetAllItemsAsync<CosmosItemPrice>();
            var existingItem = existingItems.FirstOrDefault(i => i.ItemName.Equals(itemName));

            CosmosItemPrice cosmosItemPrice;

            if (existingItem != null)
            {
                cosmosItemPrice = existingItem;
                cosmosItemPrice.Prices.Add(new ItemPrice
                {
                    Price = mean,
                    TimeRecorded = DateTime.Now
                });
            }
            else
            {
                cosmosItemPrice = new CosmosItemPrice
                {  
                    id = Guid.NewGuid().ToString(),
                    ItemName = itemName,
                    Type = "Item",
                    Prices = new List<ItemPrice>
                    {   
                        new ItemPrice
                        {
                            Price = mean,
                            TimeRecorded = DateTime.UtcNow
                        }
                    }
                };
            }
            await _cosmosService.UpsertItemAsync(cosmosItemPrice, cosmosItemPrice.ItemName);

            log.LogInformation($"Mageblood finished at: {DateTime.UtcNow}");
        }
    }
}