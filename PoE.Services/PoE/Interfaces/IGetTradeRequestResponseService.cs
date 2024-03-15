using PoE.Services.Models.PoE;

namespace PoE.Services;

public interface IGetTradeRequestResponseService
{
    Task<PoETradeRequestResponse> GetTradeRequestResponse(string itemName);
    Task<PoETradeItemResponse> GetTradeItemResponse(string requestString);
}