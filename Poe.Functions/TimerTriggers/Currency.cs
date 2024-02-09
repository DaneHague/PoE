using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Poe.Redis;
using PoE.Services;
using PoE.Services.Models.Cosmos.PoE.CosmosCurrencyItems;

namespace Poe.Functions.Triggers;

public class Currency
{
    private readonly IGetCurrencyItems _getCurrencyItems;
    private readonly ICosmosService _cosmosService;
    private readonly RedisClient _redisClient;

    public Currency(
            IGetCurrencyItems getCurrencyItems,
            ICosmosService cosmosService,
            RedisClient redisClient)
    {
        _getCurrencyItems = getCurrencyItems;
        _cosmosService = cosmosService;
        _redisClient = redisClient;
    }
    
    [FunctionName("CurrencyTrigger")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Currency trigger started at {DateTime.Now}");
    
        var items = await _getCurrencyItems.GetAllCurrencyItems();
        
        foreach (CosmosCurrencyItems currencyItem in items.Stash.Items)
        {
            var fields = new Dictionary<string, string>
            {
                {"Name", currencyItem.Name},
                {"StackSize", currencyItem.StackSize.ToString()}
            };
            
            _redisClient.SetHash($"CurrencyItem:{currencyItem.id}", fields);
            
            await _cosmosService.UpsertItemAsync(currencyItem, currencyItem.id);
        }
        
        log.LogInformation($"Currency trigger finished at {DateTime.Now}");
    }
}