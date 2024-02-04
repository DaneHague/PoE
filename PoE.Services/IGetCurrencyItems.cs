using PoE.Services.Models.Cosmos.PoE;

namespace PoE.Services;

public interface IGetCurrencyItems
{
    Task<StashResponse> GetAllCurrencyItems();
    Task<string> GetCurrencyStashId();
}