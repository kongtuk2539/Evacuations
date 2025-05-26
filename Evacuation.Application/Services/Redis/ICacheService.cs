namespace Evacuations.Application.Services.Redis;

public interface ICacheService<T> where T : class
{
    public Task<bool> IsExistsAsync(string key);
    public Task SaveAsync(string key, IEnumerable<T> value);
    public Task<IEnumerable<T>> GetAsync(string key);
    public Task RemoveAsync(string key);
}
