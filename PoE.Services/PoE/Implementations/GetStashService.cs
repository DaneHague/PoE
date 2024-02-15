using System.Text.Json;
using PoE.Services.Models;

namespace PoE.Services.Implementations;

public class GetStashService : IGetStashService
{
    private readonly HttpClient _httpClient;
    private readonly ICosmosService _cosmosService;

    public GetStashService(
        HttpClient httpClient,
        ICosmosService cosmosService)
    {
        _httpClient = httpClient;
        _cosmosService = cosmosService;
    }
    
    
    public async Task<Stash> GetAllStashTabs()
    {
        Uri t = new Uri(_httpClient.BaseAddress, "/stash/affliction");
        var response = await _httpClient.GetAsync(t);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Error fetching stash tabs");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Stash>(content);
    }
    
    public async Task<string> GetStashByType(string type)
    {
        var x = await _cosmosService.GetItemsAsyncQuery<CosmosStash>($"SELECT * FROM c WHERE c.Type = '{type}'");
        return x.First().id;
    }
}