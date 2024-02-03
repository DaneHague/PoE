namespace PoE.Services.Models.Cosmos;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ContainerAttribute : Attribute
{
    public string Name { get; }

    public ContainerAttribute(string name)
    {
        Name = name;
    }
}