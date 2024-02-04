using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models.Cosmos.PoE.CosmosStashItems;

namespace Poe.Functions.Triggers;

public class Essence
{
    private readonly IGetEssenceItems _getEssenceItems;
    private readonly ICosmosService _cosmosService;

    public Essence(
        IGetEssenceItems getEssenceItems,
        ICosmosService cosmosService)
    {
        _getEssenceItems = getEssenceItems;
        _cosmosService = cosmosService;
    }
    
    [FunctionName("EssenceTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Essence trigger started at {DateTime.Now}");
        
        var items = await _getEssenceItems.GetAllEssenceItems();

        foreach (CosmosEssenceItems essenceItem in items.Stash.Items)
        {
            await _cosmosService.UpsertItemAsync(essenceItem, essenceItem.id);
        }
        
        log.LogInformation($"Essence trigger finished at {DateTime.Now}");
    }
}