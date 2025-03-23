using Microsoft.EntityFrameworkCore.Query;
using System.Text;
using System.Text.Json;

namespace IRecharge_API.Cache_Management_Service
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ICacheservice _cacheService;
        private const string CacheKey = "AccessToken";

        public TokenService(HttpClient httpClient, IConfiguration configuration, ICacheservice cacheService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cacheService = cacheService;
        }

        // Private method to fetch token from external API
        private async Task<string> FetchTokenFromExternalApiAsync()
        {
            var loginEndpoint = _configuration["ExternalAPI:LoginEndpoint"];
            var credentials = new
            {
                username = _configuration["ExternalAPI:username"],
                password = _configuration["ExternalAPI:password"]
            };
            var request = new { username = credentials.username, password = credentials.password };
            var content = new StringContent(JsonSerializer.Serialize(credentials), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(loginEndpoint, content);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);
            var expiration = TimeSpan.FromMinutes(tokenResponse.ExpiresInMinutes);

            // Cache the token
            await _cacheService.SetAsync(CacheKey, tokenResponse.Token,expiration);

            return tokenResponse.Token;
        }

        // Public method to get token
        public async Task<string> GetTokenAsync()
        {
            // Check cache for token
            var cachedToken = await _cacheService.GetAsync<string>(CacheKey);
            if (!string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            // Fetch new token if cache is empty
            return await FetchTokenFromExternalApiAsync();
        }

        public async Task SetTokenAsync (string token, int expiresInMinutes)
        {
            if (expiresInMinutes <= 86400)
            {
                 throw new ArgumentOutOfRangeException(nameof(expiresInMinutes),"Expiration time must be a positive value.");
            }

            await _cacheService.SetAsync(CacheKey, token, TimeSpan.FromMinutes(expiresInMinutes));
        }

        private class TokenResponse
        {
            public string Token { get; set; }
            public int ExpiresInMinutes { get; set; }
        }
    }
}
