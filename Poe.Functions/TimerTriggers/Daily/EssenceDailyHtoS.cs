using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class EssenceDailyHtoS
{
    private readonly ILogger<EssenceDailyHtoS> _log;
    private readonly ITimerTriggerService _timerTriggerService;

    public EssenceDailyHtoS(ILogger<EssenceDailyHtoS> log, ITimerTriggerService timerTriggerService)
    {
        _log = log;
        _timerTriggerService = timerTriggerService;
    }

    [FunctionName("EssenceDailyHtoS")]
    public async Task RunAsync([TimerTrigger("0 15 17 * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _log.LogInformation($"EssenceDailyHtoS started at: {DateTime.UtcNow}");

        List<string> essenceList = new()
        {
            "Deafening Essence of Hatred",
            "Essence of Horror",
            "Essence of Hysteria",
            "Essence of Insanity",
            "Deafening Essence of Loathing",
            "Deafening Essence of Misery",
            "Deafening Essence of Rage",
            "Deafening Essence of Scorn",
            "Deafening Essence of Sorrow",
            "Deafening Essence of Spite",
            "Deafening Essence of Suffering"
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
        
        _log.LogInformation($"EssenceDailyHtoS finished at: {DateTime.UtcNow}");
    }
}