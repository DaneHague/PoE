using PoE.Services.Models.Cosmos.PoE.CosmosStashItems;

namespace PoE.Services;

public interface IGetEssenceItems
{
    Task<EssenceStashResponse> GetAllEssenceItems();
}