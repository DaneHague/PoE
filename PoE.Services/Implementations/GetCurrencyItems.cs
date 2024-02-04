using System.Text.Json;
using PoE.Services.Models;
using PoE.Services.Models.Cosmos.PoE;

namespace PoE.Services.Implementations;

public class GetCurrencyItems : IGetCurrencyItems
{
    private readonly HttpClient _httpClient;
    private readonly ICosmosService _cosmosService;

    public GetCurrencyItems(
        HttpClient httpClient,
        ICosmosService cosmosService)
    {
        _httpClient = httpClient;
        _cosmosService = cosmosService;
    }

    public async Task<StashResponse> GetAllCurrencyItems()
    {
        string tabId = await GetCurrencyStashId();
        Uri t = new Uri(_httpClient.BaseAddress, $"/stash/affliction/{tabId}");
        var response = await _httpClient.GetAsync(t);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Error fetching stash tabs");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StashResponse>(content);
    }

    public async Task<string> GetCurrencyStashId()
    {
        var x = await _cosmosService.GetItemsAsyncQuery<CosmosStash>("SELECT * FROM c WHERE c.Type = 'CurrencyStash'");
        return x.First().id;
    }
}