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
    private readonly IGetTradeRequestResponseService _getTradeRequestResponseService;

    public MageBloodDay(
        ICosmosService cosmosService,
        IGetTradeRequestResponseService getTradeRequestResponseService)
    {
        _cosmosService = cosmosService;
        _getTradeRequestResponseService = getTradeRequestResponseService;
    }
    
    [FunctionName("MageBloodDay")]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
        
        var tradeRequestResponses = await _getTradeRequestResponseService.GetTradeRequestResponse("Mageblood");
        var x = 1;
    }
}