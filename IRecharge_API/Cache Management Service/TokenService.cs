using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IRecharge_API.Cache_Management_Service
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ICacheservice _cacheService;
        private readonly ILogger<TokenService> _logger;
        private const string CacheKey = "AccessToken";

        public TokenService(
            HttpClient httpClient,
            IConfiguration configuration,
            ICacheservice cacheService,
            ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cacheService = cacheService;
            _logger = logger;
        }

        // Private method to fetch token from external API
        private async Task<string> FetchTokenFromExternalApiAsync()
        {
            try
            {
                var loginEndpoint = _configuration["ExternalAPI:LoginEndpoint"];
                var credentials = new
                {
                    username = _configuration["ExternalAPI:username"],
                    password = _configuration["ExternalAPI:password"]
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(credentials),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(loginEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);

                // Validate expiration time
                if (tokenResponse.ExpiresInMinutes <= 0)
                {
                    _logger.LogWarning(
                        "Invalid expiration time {ExpiresInMinutes} received from API. Using default 30 minutes.",
                        tokenResponse.ExpiresInMinutes);
                    tokenResponse.ExpiresInMinutes = 30; // Default fallback
                }

                // Cache the token
                var success = await SetTokenAsync(tokenResponse.Token, tokenResponse.ExpiresInMinutes);
                if (!success)
                {
                    _logger.LogError("Failed to cache the token");
                }

                return tokenResponse.Token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching token from external API");
                throw;
            }
        }

        // Public method to get token
        public async Task<string> GetTokenAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting token");
                throw;
            }
        }

        public async Task<bool> SetTokenAsync(string token, int expiresInMinutes)
        {
            try
            {
                if (expiresInMinutes <= 0)
                {
                    _logger.LogWarning(
                        "Attempted to set token with invalid expiration: {ExpiresInMinutes} minutes",
                        expiresInMinutes);
                    return false;
                }

                await _cacheService.SetAsync(
                    CacheKey,
                    token,
                    TimeSpan.FromMinutes(expiresInMinutes));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error caching token");
                return false;
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
            public int ExpiresInMinutes { get; set; }
        }
    }
}