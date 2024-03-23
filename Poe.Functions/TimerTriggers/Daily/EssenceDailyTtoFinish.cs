using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class EssenceDailyTtoFinish
{
    private readonly ILogger<EssenceDailyTtoFinish> _log;
    private readonly ITimerTriggerService _timerTriggerService;

    public EssenceDailyTtoFinish(ILogger<EssenceDailyTtoFinish> log, ITimerTriggerService timerTriggerService)
    {
        _log = log;
        _timerTriggerService = timerTriggerService;
    }

    [FunctionName("EssenceDailyTtoFinish")]
    public async Task RunAsync([TimerTrigger("0 30 17 * * *")] TimerInfo myTimer)
    {
        _log.LogInformation($"EssenceDailyTtoFinish started at: {DateTime.UtcNow}");

        List<string> essenceList = new()
        {
            "Deafening Essence of Torment",
            "Deafening Essence of Woe",
            "Deafening Essence of Wrath",
            "Deafening Essence of Zeal"

        };
        
        for (int i = 0; i < essenceList.Count; i++)
        {
            var prices = await _timerTriggerService.FetchTradeResponsesAndCalculateMeanPrice(essenceList[i], 2, true);

            if (prices.Any())
            {
                await _timerTriggerService.UpsertItemPrice(essenceList[i], prices);
            }
            
            // Wait for 10 seconds to avoid rate limiting
            await Task.Delay(TimeSpan.FromSeconds(20));
        }

        _log.LogInformation($"EssenceDailyTtoFinish finished at: {DateTime.UtcNow}");
    }
}