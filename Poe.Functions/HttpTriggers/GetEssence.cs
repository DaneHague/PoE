using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models.Cosmos.PoE.CosmosStashItems;

namespace Poe.Functions.HttpTriggers;

public class GetEssence
{
    private readonly ICosmosService _cosmosService;
    public GetEssence(
        ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }
    
    [FunctionName("GetEssence")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetEssence")] HttpRequest req,
        ILogger log)
    {
        try
        {
            var query = "SELECT * FROM c WHERE c.Type = 'Essence'";
            var result = await _cosmosService.GetItemsAsyncQuery<CosmosEssenceItems>(query);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError($"Error retrieving essence items: {ex.Message}");
            return new StatusCodeResult(500); // Internal Server Error
        }
    }
}