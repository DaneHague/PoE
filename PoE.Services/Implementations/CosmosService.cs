using Microsoft.Azure.Cosmos;
using Poe.Functions.Config;
using PoE.Services.Models.Cosmos;

namespace PoE.Services.Implementations;

public class CosmosService : ICosmosService
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public CosmosService(
        CosmosConfig config,
        CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _container = _cosmosClient.GetContainer(config.Database, "StashContainer");
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
}