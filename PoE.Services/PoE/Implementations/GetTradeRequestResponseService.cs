using System.Text;
using System.Text.Json;
using PoE.Services.Models.PoE;
using PoE.Services.Models.PoE.Currency;
using PoE.Services.Models.PoE.Item;
using Currency_Query = PoE.Services.Models.PoE.Currency.Query;
using Currency_Status = PoE.Services.Models.PoE.Currency.Status;
using Item_Query = PoE.Services.Models.PoE.Item.Query;
using Item_Sort = PoE.Services.Models.PoE.Item.Sort;
using Item_Status = PoE.Services.Models.PoE.Item.Status;

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
            Query = new Item_Query(){
                Name = itemName,
                Status = new Item_Status()
                {
                    Option = "online"
                },
                Stats = new List<ItemStat>
                {
                    new ItemStat()
                    {
                        Type = "and",
                        Filters = new List<object>()
                    }
                }
            },
            Sort = new Item_Sort()
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
    
    public async Task<PoETradeRequestResponse> GetTradeRequestResponseCurrency(string itemName)
    {
        PoETradeRequestCurrency poETradeRequest = new PoETradeRequestCurrency()
        {
            Query = new Currency_Query()
            {
                Status = new Currency_Status()
                {
                    Option = "online"
                },
                Term = itemName,
                Filters = new Filters
                {
                    TradeFilters = new TradeFilters
                    {
                        Filters = new TradeFilterDetails
                        {
                            Category = new Category
                            {
                                Option = "currency"
                            }
                        }
                    }
                }
            },
            Sort = new Models.PoE.Currency.Sort()
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
    
    public async Task<PoETradeItemResponse> GetTradeItemResponse(string requestString)
    {
        var response = _httpClient.GetAsync($"https://www.pathofexile.com/api/trade/fetch/{requestString}");
        
        if (response.Result.IsSuccessStatusCode)
        {
            var responseContent = await response.Result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PoETradeItemResponse>(responseContent);
        }

        return null;
    }
}