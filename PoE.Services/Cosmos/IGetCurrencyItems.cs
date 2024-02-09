using PoE.Services.Models.Cosmos.PoE;
using PoE.Services.Models.Cosmos.PoE.CosmosCurrencyItems;

namespace PoE.Services;

public interface IGetCurrencyItems
{
    Task<CurrencyStashResponse> GetAllCurrencyItems();
    
}