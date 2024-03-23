using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using PoE.Services;
using PoE.Services.Models.PoE.CosmosItemPrice;

namespace Poe.Functions.Triggers;

public class MagebloodDaily
{
    private readonly ILogger<MagebloodDaily> _log;
    private readonly ITimerTriggerService _timerTriggerService;

    public MagebloodDaily(ILogger<MagebloodDaily> log, ITimerTriggerService timerTriggerService)
    {
        _log = log;
        _timerTriggerService = timerTriggerService;
    }

    [FunctionName("MageBloodDaily")]
    public async Task RunAsync([TimerTrigger("0 0 18 * * *")] TimerInfo myTimer)
    {
        _log.LogInformation($"MagebloodDaily started at: {DateTime.UtcNow}");

        var prices = await _timerTriggerService.FetchTradeResponsesAndCalculateMeanPrice("Mageblood", 5);

        if (prices.Any())
        {
            await _timerTriggerService.UpsertItemPrice("MageBlood", prices);
        }

        _log.LogInformation($"MagebloodDaily finished at: {DateTime.UtcNow}");
    }
}