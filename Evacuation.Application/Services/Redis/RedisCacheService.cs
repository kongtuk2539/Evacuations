using Evacuations.Application.Dtos.Evacuations.Responses;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Evacuations.Application.Services.Redis;

public class RedisCacheService(IDistributedCache cache) : ICacheService<EvacuationStatusResponseDto>
{
    private readonly TimeSpan cacheExpiry = TimeSpan.FromMinutes(30);
    private readonly TimeSpan resetTime = TimeSpan.FromMinutes(30);

    public async Task<bool> IsExistsAsync(string key)
    {
        return await cache.GetStringAsync(key) is not null;
    }

    public async Task<IEnumerable<EvacuationStatusResponseDto>> GetAsync(string key)
    {
        var keySearch = await cache.GetStringAsync(key);
        return JsonConvert.DeserializeObject<IEnumerable<EvacuationStatusResponseDto>>(keySearch!)!;
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task SaveAsync(string key, IEnumerable<EvacuationStatusResponseDto> value)
    {
        await cache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(value),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheExpiry,
                    SlidingExpiration = resetTime
                }
            );
    }
}
