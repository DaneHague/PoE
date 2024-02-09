using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Poe.Redis;

namespace Poe.Functions.HttpTriggers;

public class GetEssence
{
    private readonly RedisClient _redisClient;

    public GetEssence(RedisClient redisClient)
    {
        _redisClient = redisClient;
    }
    
    [FunctionName("GetEssence")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetEssence")] HttpRequest req,
        ILogger log)
    {
        try
        {
            var result = await _redisClient.GetHashesByPattern("EssenceItem:*");
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError($"Error retrieving essence items: {ex.Message}");
            return new StatusCodeResult(500); // Internal Server Error
        }
    }
}