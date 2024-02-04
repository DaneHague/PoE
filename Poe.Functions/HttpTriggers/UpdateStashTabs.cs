using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.HttpTriggers;

public class UpdateStashTabs
{
    private readonly IGetStashService _getStashService;
    private readonly ICosmosService _cosmosService;

    public UpdateStashTabs(
        IGetStashService getStashService,
        ICosmosService cosmosService
        )
    {
        _getStashService = getStashService;
        _cosmosService = cosmosService;
    }
    
    [FunctionName("UpdateStashTabs")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "UpdateStashTabs")] HttpRequest req, ILogger log)
    {
        log.LogInformation($"Update Stash Tabs trigger started at {DateTime.Now}");

        var stashes = await _getStashService.GetAllStashTabs();
        
        foreach (var stash in stashes.Stashes)
        {
            await _cosmosService.UpdateItemAsync(stash, stash.id);
        }
        
        log.LogInformation($"Update Stash Tabs trigger finished at {DateTime.Now}");
    }
}