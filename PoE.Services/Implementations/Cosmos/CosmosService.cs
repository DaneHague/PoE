using System.Reflection;
using Microsoft.Azure.Cosmos;
using Poe.Functions.Config;
using PoE.Services.Models.Cosmos;

namespace PoE.Services.Implementations;

public class CosmosService : ICosmosService
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;
    private readonly CosmosConfig _config;

    public CosmosService(
        CosmosConfig config,
        CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _config = config;
    }

    public string GetContainerName<T>()
    {
        var attr = (ContainerAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ContainerAttribute));
        return attr?.Name;
    }

    public async Task<T> CreateItemAsync<T>(T item, string partitionKey) where T : ICosmosEntity
    {
        return await _container.CreateItemAsync(item, new PartitionKey(partitionKey));
    }

    public async Task<T> UpsertItemAsync<T>(T item, string partitionKey) where T : ICosmosEntity
    {
        var containerAttribute = typeof(T).GetCustomAttribute<ContainerAttribute>();
        if (containerAttribute == null)
        {
            throw new InvalidOperationException($"The {typeof(T).Name} class does not have a Container attribute.");
        }
        
        var containerName = containerAttribute.Name;
        
        Container container = _cosmosClient.GetContainer(_config.Database, containerName);
        
        return await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
    }

    public async Task<T> GetItemAsync<T>(string id, string partitionKey) where T : ICosmosEntity
    {
        return await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
    }

    public async Task<IEnumerable<T>> GetItemsAsyncQuery<T>(string queryString) where T : ICosmosEntity
    {
        var containerAttribute = typeof(T).GetCustomAttribute<ContainerAttribute>();
        if (containerAttribute == null)
        {
            throw new InvalidOperationException($"The {typeof(T).Name} class does not have a Container attribute.");
        }
        
        var containerName = containerAttribute.Name;
        
        Container container = _cosmosClient.GetContainer(_config.Database, containerName);
        return await container.GetItemQueryIterator<T>(new QueryDefinition(queryString)).ReadNextAsync();
    }
}