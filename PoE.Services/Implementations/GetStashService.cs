using System.Text.Json;
using PoE.Services.Models;

namespace PoE.Services.Implementations;

public class GetStashService : IGetStashService
{
    private readonly HttpClient _httpClient;

    public GetStashService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}