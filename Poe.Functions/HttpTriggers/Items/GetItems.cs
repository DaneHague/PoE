using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PoE.Services;
using CosmosItem = PoE.Services.Models.PoE.CosmosItem;

namespace Poe.Functions.HttpTriggers.Items;

public class GetItems
{
    private ICosmosService _cosmosService;

    public GetItems(ICosmosService cosmosService)
    {
        _cosmosService = cosmosService;
    }

    [FunctionName("GetItems")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "GetItems")]
        HttpRequest req,
        ILogger log)
    {
        var items = await _cosmosService.GetAllItemsAsync<CosmosItem>();
        return new OkObjectResult(items);
    }
}