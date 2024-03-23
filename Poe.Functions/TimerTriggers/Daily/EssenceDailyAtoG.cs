using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class EssenceDailyAtoG
{
    private readonly ILogger<EssenceDailyAtoG> _log;
    private readonly ITimerTriggerService _timerTriggerService;

    public EssenceDailyAtoG(ILogger<EssenceDailyAtoG> log, ITimerTriggerService timerTriggerService)
    {
        _log = log;
        _timerTriggerService = timerTriggerService;
    }

    [FunctionName("EssenceDailyAtoG")]
    public async Task RunAsync([TimerTrigger("0 0 17 * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        _log.LogInformation($"EssenceDailyAtoG started at: {DateTime.UtcNow}");

        List<string> essenceList = new()
        {
            "Deafening Essence of Anger",
            "Deafening Essence of Anguish",
            "Deafening Essence of Contempt",
            "Essence of Delirium",
            "Deafening Essence of Doubt",
            "Deafening Essence of Dread",
            "Deafening Essence of Envy",
            "Deafening Essence of Fear",
            "Deafening Essence of Greed"
        };

        for (int i = 0; i < essenceList.Count; i++)
        {
            var prices = await _timerTriggerService.FetchTradeResponsesAndCalculateMeanPrice(essenceList[i], 3, true);

            if (prices.Any())
            {
                await _timerTriggerService.UpsertItemPrice(essenceList[i], prices);
            }
        }

        _log.LogInformation($"EssenceDailyAtoG finished at: {DateTime.UtcNow}");
    }
}