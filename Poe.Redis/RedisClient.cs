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
    
    public void SetHash(string key, Dictionary<string, string> hashFields)
    {
        var entries = hashFields.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
        _db.HashSet(key, entries);
    }
    
    public async Task<Dictionary<string, Dictionary<string, string>>> GetHashesByPattern(string pattern)
    {
        var server = GetServer();
        var keys = server.Keys(_db.Database, pattern);
        var hashes = new Dictionary<string, Dictionary<string, string>>();

        foreach (var key in keys)
        {
            if (_db.KeyType(key) == RedisType.Hash)
            {
                var hashEntries = _db.HashGetAll(key);
                var hashFields = hashEntries.ToDictionary(he => he.Name.ToString(), he => he.Value.ToString());
                hashes.Add(key, hashFields);
            }
        }
        
        return hashes;
    }
    
    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints().First();
        return _redis.GetServer(endpoint);
    }
}