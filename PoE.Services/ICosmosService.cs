using PoE.Services.Models.Cosmos;

namespace PoE.Services;

public interface ICosmosService
{
    Task<T> CreateItemAsync<T>(T item, string partitionKey) where T : ICosmosEntity;
}