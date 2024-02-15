using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Poe.Redis;
using PoE.Services;
using PoE.Services.Models.Cosmos.PoE.CosmosCurrencyItems;

namespace Poe.Functions.HttpTriggers;

public class GetCurrency
{
    private readonly ICosmosService _cosmosService;
    public GetCurrency(
        ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }
    
    [FunctionName("GetCurrency")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCurrency")] HttpRequest req,
        ILogger log)
    {
        try
        {
            var query = "SELECT * FROM c WHERE c.Type = 'Currency'";
            
            var result = await _cosmosService.GetItemsAsyncQuery<CosmosCurrencyItems>(query);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError($"Error retrieving currency items: {ex.Message}");
            return new StatusCodeResult(500); // Internal Server Error
        }
    }
}