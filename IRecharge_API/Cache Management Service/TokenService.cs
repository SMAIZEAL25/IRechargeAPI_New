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
                Username = _configuration["ExternalAPI:username"],
                Password = _configuration["ExternalAPI:password"]
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(loginEndpoint, content);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);

            // Cache the token
            await _cacheService.SetAsync(CacheKey, tokenResponse.Token, TimeSpan.FromMinutes(tokenResponse.ExpiresInMinutes));

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

        private class TokenResponse
        {
            public string Token { get; set; }
            public int ExpiresInMinutes { get; set; }
        }
    }
}
