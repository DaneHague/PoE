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
        _container = _cosmosClient.GetContainer(config.Database, "Items");
    }

    public string GetContainerName<T>()
    {
        var attr = (ContainerAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ContainerAttribute));
        return attr?.Name;
    }

    public void tst()
    {
        var containerName = GetContainerName<CosmosItem>();
        var x = 2;
    }
}