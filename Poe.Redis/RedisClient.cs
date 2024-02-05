using StackExchange.Redis;

namespace Poe.Redis;

public class RedisClient
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    
    public RedisClient(RedisConfig config)
    {
        var options = ConfigurationOptions.Parse($"{config.IpAddress}:{config.Port},password={config.Password}");
        _redis = ConnectionMultiplexer.Connect(options);
        _db = _redis.GetDatabase();
    }
    
    public void SetValue(string key, string value)
    {
        _db.StringSet(key, value);
    }

    public string GetValue(string key)
    {
        return _db.StringGet(key);
    }
    
}