using System.Text.Json;
using PoE.Services.Models;
using PoE.Services.Models.Cosmos.PoE;
using PoE.Services.Models.Cosmos.PoE.CosmosCurrencyItems;

namespace PoE.Services.Implementations;

public class GetCurrencyItems : IGetCurrencyItems
{
    private readonly HttpClient _httpClient;
    private readonly IGetStashService _getStashService;

    public GetCurrencyItems(
        HttpClient httpClient,
        IGetStashService getStashService)
    {
        _httpClient = httpClient;
        _getStashService = getStashService;
    }

    public async Task<CurrencyStashResponse> GetAllCurrencyItems()
    {
        string tabId = await _getStashService.GetStashByType("CurrencyStash");
        Uri t = new Uri(_httpClient.BaseAddress, $"/stash/affliction/{tabId}");
        var response = await _httpClient.GetAsync(t);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Error fetching stash tabs");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CurrencyStashResponse>(content);
    }

    
}