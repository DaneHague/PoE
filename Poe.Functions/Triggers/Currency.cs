using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class Currency
{
    private readonly IGetProfileInfo _getProfileInfo;

    public Currency(IGetProfileInfo getProfileInfo)
    {
        _getProfileInfo = getProfileInfo;
    }
    
    [FunctionName("CurrencyTrigger")]
    public static void Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Currency trigger started at {DateTime.Now}");
    }
}