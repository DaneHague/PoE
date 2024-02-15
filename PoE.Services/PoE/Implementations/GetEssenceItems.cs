using System.Text.Json;
using PoE.Services.Models.Cosmos.PoE.CosmosStashItems;

namespace PoE.Services.Implementations;

public class GetEssenceItems : IGetEssenceItems
{
    private readonly IGetStashService _getStashService;
    private readonly HttpClient _httpClient;

    public GetEssenceItems(
        HttpClient httpClient,
        IGetStashService getStashService)
    {
        _httpClient = httpClient;
        _getStashService = getStashService;
    }
    
    public async Task<EssenceStashResponse> GetAllEssenceItems()
    {
        string tabId = await _getStashService.GetStashByType("EssenceStash");
        Uri t = new Uri(_httpClient.BaseAddress, $"/stash/affliction/{tabId}");
        var response = await _httpClient.GetAsync(t);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Error fetching stash tabs");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EssenceStashResponse>(content);
    }
}