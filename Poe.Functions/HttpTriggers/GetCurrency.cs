using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Poe.Redis;

namespace Poe.Functions.HttpTriggers;

public class GetCurrency
{
    private readonly RedisClient _redisClient;
    public GetCurrency(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }
    
    [FunctionName("GetCurrency")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCurrency")] HttpRequest req,
        ILogger log)
    {
        try
        {
            var result = await _redisClient.GetHashesByPattern("CurrencyItem:*");
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError($"Error retrieving currency items: {ex.Message}");
            return new StatusCodeResult(500); // Internal Server Error
        }
    }
}