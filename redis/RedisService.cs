using StackExchange.Redis;

public class RedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    public int indexDB { get; set; } = 0;

    public int expiresInSeconds { get; set; } = 3600; // Thời gian hết hạn mặc định là 1 giờ (3600 giây)

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }

    public async Task SetStringAsync(string key, string value)
    {
        var _db = _connectionMultiplexer.GetDatabase(indexDB);
        await _db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiresInSeconds));
    }

    public async Task<string> GetStringAsync(string key)
    {
        var _db = _connectionMultiplexer.GetDatabase(indexDB);
        return await _db.StringGetAsync(key);
    }

    public async Task RemoveKeyAsync(string key)
    {
        var _db = _connectionMultiplexer.GetDatabase(indexDB);
        await _db.KeyDeleteAsync(key);
    }
}