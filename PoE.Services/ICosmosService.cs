using PoE.Services.Models.Cosmos;

namespace PoE.Services;

public interface ICosmosService
{
    Task<T> CreateItemAsync<T>(T item, string partitionKey) where T : ICosmosEntity;
    Task<T> UpsertItemAsync<T>(T item, string partitionKey) where T : ICosmosEntity;
    Task<T> GetItemAsync<T>(string id, string partitionKey) where T : ICosmosEntity;
    Task<IEnumerable<T>> GetItemsAsyncQuery<T>(string queryString) where T : ICosmosEntity;
}