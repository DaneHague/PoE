using PoE.Services.Models.PoE;

namespace PoE.Services.Implementations;

public class GetTradeRequestResponseService : IGetTradeRequestResponseService
{
    private readonly HttpClient _httpClient;

    public GetTradeRequestResponseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PoETradeRequestResponse> GetTradeRequestResponse()
    {
        // Create body and make request
        return null;
    }
}