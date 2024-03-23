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

public class MagebloodHourly
{
    private readonly ICosmosService _cosmosService;
    private readonly IGetTradeRequestResponseService _getTradeRequestResponseService;

    public MagebloodHourly(
        ICosmosService cosmosService,
        IGetTradeRequestResponseService getTradeRequestResponseService)
    {
        _cosmosService = cosmosService;
        _getTradeRequestResponseService = getTradeRequestResponseService;
    }
    
    [FunctionName("MageBloodHourly")]
    public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Mageblood started at: {DateTime.UtcNow}");
        
        var tradeRequestResponses = await _getTradeRequestResponseService.GetTradeRequestResponse("Mageblood");
        
        if (tradeRequestResponses != null)
        {
            int loopLength = tradeRequestResponses.Result.Count >= 5 ? 5 : tradeRequestResponses.Result.Count;
            List<decimal> prices = new List<decimal>();
            
            for (int i = 0; i < loopLength; i++)
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

            log.LogInformation($"Mageblood started at: {DateTime.UtcNow}");
        }
    }
}