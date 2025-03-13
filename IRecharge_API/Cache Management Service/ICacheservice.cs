namespace IRecharge_API.Cache_Management_Service
{
    public interface ICacheservice
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value, TimeSpan expiration);
    }
}
