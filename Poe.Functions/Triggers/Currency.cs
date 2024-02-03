using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PoE.Services;

namespace Poe.Functions.Triggers;

public class Currency
{
    private readonly IGetProfileInfo _getProfileInfo;
    private readonly IGetStashService _getStashService;

    public Currency(
            IGetProfileInfo getProfileInfo, 
            IGetStashService getStashService)
    {
        _getProfileInfo = getProfileInfo;
        _getStashService = getStashService;
    }
    
    [FunctionName("CurrencyTrigger")]
    public void Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Currency trigger started at {DateTime.Now}");
        
        _getStashService.GetAllStashTabs();
        
        log.LogInformation($"Currency trigger finished at {DateTime.Now}");
    }
}