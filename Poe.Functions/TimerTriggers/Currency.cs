using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models;
using PoE.Services.Models.Cosmos.PoE;
using PoE.Services.Models.Cosmos.PoE.CosmosCurrencyItems;

namespace Poe.Functions.Triggers;

public class Currency
{
    private readonly IGetCurrencyItems _getCurrencyItems;
    private readonly ICosmosService _cosmosService;

    public Currency(
            IGetCurrencyItems getCurrencyItems,
            ICosmosService cosmosService)
    {
        _getCurrencyItems = getCurrencyItems;
        _cosmosService = cosmosService;
    }
    
    [FunctionName("CurrencyTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Currency trigger started at {DateTime.Now}");
    
        var items = await _getCurrencyItems.GetAllCurrencyItems();
        
        foreach (CosmosCurrencyItems currencyItem in items.Stash.Items)
        {
            await _cosmosService.UpsertItemAsync(currencyItem, currencyItem.id);
        }
        
        log.LogInformation($"Currency trigger finished at {DateTime.Now}");
    }
}