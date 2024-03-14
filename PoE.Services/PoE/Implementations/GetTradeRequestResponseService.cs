using System.Text;
using System.Text.Json;
using PoE.Services.Models.PoE;
using PoE.Services.Models.PoE.PoETradeRequest;

namespace PoE.Services.Implementations;

public class GetTradeRequestResponseService : IGetTradeRequestResponseService
{
    private readonly HttpClient _httpClient;

    public GetTradeRequestResponseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PoETradeRequestResponse> GetTradeRequestResponse(string itemName)
    {
        PoETradeRequest poETradeRequest = new PoETradeRequest
        {
            Query = new Query
            {
                Name = itemName,
                Status = new Status
                {
                    Option = "online"
                },
                Stats = new List<Stat>
                {
                    new Stat
                    {
                        Type = "and",
                        Filters = new List<object>()
                    }
                }
            },
            Sort = new Sort
            {
                Price = "asc"
            }
        };

        var body = JsonSerializer.Serialize(poETradeRequest);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PoETradeRequestResponse>(responseContent);
        }
        
        return null;
    }
}