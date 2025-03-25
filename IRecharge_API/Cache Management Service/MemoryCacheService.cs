using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace IRecharge_API.Cache_Management_Service
{
    public class MemoryCacheService : ICacheservice
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public Task<T> GetAsync<T>(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Cache key cannot be null or whitespace", nameof(key));
                }

                if (_memoryCache.TryGetValue(key, out T value))
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                    return Task.FromResult(value);
                }

                _logger.LogDebug("Cache miss for key: {Key}", key);
                return Task.FromResult(default(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cached value for key: {Key}", key);
                throw;
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Cache key cannot be null or whitespace", nameof(key));
                }

                if (expiration <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(expiration), "Expiration time must be positive");
                }

                if (value == null)
                {
                    _logger.LogWarning("Attempted to cache null value for key: {Key}", key);
                    return Task.CompletedTask;
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiration);

                _memoryCache.Set(key, value, cacheOptions);
                _logger.LogDebug("Cached value for key: {Key} with expiration: {Expiration}", key, expiration);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cached value for key: {Key}", key);
                throw;
            }
        }
    }
}