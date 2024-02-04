using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PoE.Services;
using PoE.Services.Models;

namespace Poe.Functions.HttpTriggers;

public class GetStashTabs
{
    private readonly IGetStashService _getStashService;
    private readonly ICosmosService _cosmosService;

    public GetStashTabs(
        IGetStashService getStashService,
        ICosmosService cosmosService)
    {
        _getStashService = getStashService;
        _cosmosService = cosmosService;
    }
    
    [FunctionName("GetStashTabs")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetStashTabs")] HttpRequest req, ILogger log)
    {
        log.LogInformation($"Stash tabs trigger started at {DateTime.Now}");
        
        var stashes = await _getStashService.GetAllStashTabs();

        foreach (CosmosStash stash in stashes.Stashes)
        {
            await _cosmosService.CreateItemAsync(stash, stash.id);
        }
        
        log.LogInformation($"Stash tabs trigger finished at {DateTime.Now}");
    }

}