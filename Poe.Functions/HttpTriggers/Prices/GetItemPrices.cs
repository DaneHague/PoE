using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PoE.Services;
using PoE.Services.Models.Cosmos;
using PoE.Services.Models.PoE.CosmosItemPrice;

namespace Poe.Functions.HttpTriggers.Prices;

public class GetItemPrices
{
    private ICosmosService _cosmosService;

    public GetItemPrices(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }
    
    [FunctionName("GetItemPrices")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", 
            Route = "GetItemPrices/{itemName}/{dateFrom}/{dateTo}/{currency}")] HttpRequest req, 
        ILogger log,
        string itemName,
        string dateFrom,
        string dateTo,
        string currency)
    {
        DateTime convertedDateFrom = DateTime.Parse(dateFrom);
        DateTime convertedDateTo = DateTime.Parse(dateTo);

        if (convertedDateFrom > convertedDateTo)
        {
            return new BadRequestObjectResult("From date cannot be greater than to date.");
        }

        var items = await _cosmosService.GetAllItemsAsync<CosmosItemPrice>();

        if (!items.Any())
        {
            return new NotFoundObjectResult("Whoops, no items in the db.");
        }

        var itemPrices = items
            .Where(c => c.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .FirstOrDefault();
        
        itemPrices.Prices = itemPrices.Prices
            .Where(p => p.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        if (itemPrices is null)
        {
            return new NotFoundObjectResult("Item doesn't exist, please check the name or there is no data.");
        }
        
        var pricesFiltered = itemPrices.Prices
            .Where(c => c.TimeRecorded >= convertedDateFrom && c.TimeRecorded <= convertedDateTo)
            .ToList();

        if (!pricesFiltered.Any())
        {
            return new NotFoundObjectResult("No data for that time period exists.");
        }

        return new OkObjectResult(pricesFiltered);
    }
}