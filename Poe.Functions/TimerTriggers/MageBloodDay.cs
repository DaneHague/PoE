using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class MageBloodDay
{
    private readonly ICosmosService _cosmosService;

    public MageBloodDay(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }
    
    [FunctionName("MageBloodDay")]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
        
    }
}