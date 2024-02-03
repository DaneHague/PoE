using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models;

namespace Poe.Functions.Triggers;

public class Currency
{
    private readonly IGetProfileInfo _getProfileInfo;
    private readonly IGetStashService _getStashService;
    private readonly ICosmosService _cosmosService;

    public Currency(
            IGetProfileInfo getProfileInfo, 
            IGetStashService getStashService,
            ICosmosService cosmosService)
    {
        _getProfileInfo = getProfileInfo;
        _getStashService = getStashService;
        _cosmosService = cosmosService;
    }
    
    [FunctionName("CurrencyTrigger")]
    public async Task Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Currency trigger started at {DateTime.Now}");
        
        var stashes = await _getStashService.GetAllStashTabs();

        foreach (Stash stash in stashes.Stashes)
        {
            await _cosmosService.CreateItemAsync(stash, stash.id);
        }
        
        log.LogInformation($"Currency trigger finished at {DateTime.Now}");
    }
}