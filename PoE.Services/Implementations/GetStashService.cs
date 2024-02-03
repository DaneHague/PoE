using PoE.Services.Models;

namespace PoE.Services.Implementations;

public class GetStashService : IGetStashService
{
    private readonly HttpClient _httpClient;

    public GetStashService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<Stash> GetAllStashTabs()
    {
        var x = _httpClient.BaseAddress;
        return null;
    }
}