using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class HeadhunterDaily
{
    private readonly ILogger _log;
    private readonly ITimerTriggerService _timerTriggerService;

    public HeadhunterDaily(ILogger log, ITimerTriggerService timerTriggerService)
    {
        _log = log;
        _timerTriggerService = timerTriggerService;
    }

    [FunctionName("HeadhunterDaily")]
    public async Task RunAsync([TimerTrigger("0 0 18 * * *")] TimerInfo myTimer)
    {
        _log.LogInformation($"HeadhunterDaily started at: {DateTime.UtcNow}");

        var prices = await _timerTriggerService.FetchTradeResponsesAndCalculateMeanPrice("Headhunter", 5);

        if (prices.Any())
        {
            await _timerTriggerService.UpsertItemPrice("Headhunter", prices);
        }

        _log.LogInformation($"HeadhunterDaily finished at: {DateTime.UtcNow}");
    }
}