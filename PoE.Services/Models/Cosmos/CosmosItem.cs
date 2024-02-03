using System.ComponentModel;

namespace PoE.Services.Models.Cosmos;

[Container("ItemContainer")]
public class CosmosItem : ICosmosEntity
{
    public string id { get; set; }
}