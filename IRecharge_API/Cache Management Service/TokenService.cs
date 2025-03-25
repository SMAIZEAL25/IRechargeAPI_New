
using System.Text;
using System.Text.Json;


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
        //private async Task<string> FetchTokenFromExternalApiAsync()
        //{
        //    try
        //    {
        //        var loginEndpoint = _configuration["ExternalAPI:LoginEndpoint"];
        //        var credentials = new
        //        {
        //            username = _configuration["ExternalAPI:username"],
        //            password = _configuration["ExternalAPI:password"]
        //        };

        //        var content = new StringContent(
        //            JsonSerializer.Serialize(credentials),
        //            Encoding.UTF8,
        //            "application/json");

        //        var response = await _httpClient.PostAsync(loginEndpoint, content);
        //        response.EnsureSuccessStatusCode();

        //        var responseData = await response.Content.ReadAsStringAsync();
        //        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);

        //        // Validate expiration time
        //        if (tokenResponse.ExpiresInMinutes <= 1)
        //        {
        //            _logger.LogWarning(
        //                "Invalid expiration time {ExpiresInMinutes} received from API. Using default 30 minutes.",
        //                tokenResponse.ExpiresInMinutes);
        //                tokenResponse.ExpiresInMinutes = 86400; // Default fallback
        //        }

        //        // Cache the token
        //        var success = await SetTokenAsync(tokenResponse.Token, tokenResponse.ExpiresInMinutes);
        //        if (!success)
        //        {
        //            _logger.LogError("Failed to cache the token");
        //        }

        //        return tokenResponse.Token;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching token from external API");
        //        throw;
        //    }
        //}

        // Public method to get token

        private async Task<string> FetchTokenFromExternalApiAsync()
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

            if (string.IsNullOrEmpty(tokenResponse?.Token))
            {
                throw new ApplicationException("Invalid token response from API");
            }

            // Cache the token
            await _cacheService.SetAsync(
                CacheKey,
                tokenResponse.Token,
                TimeSpan.FromMinutes(tokenResponse.ExpiresInMinutes));

            return tokenResponse.Token;
        }

        public async Task<string> GetApiTokenAsync()
        {
            try
            {
                var cachedToken = await _cacheService.GetAsync<string>(CacheKey);
                if (!string.IsNullOrEmpty(cachedToken))
                {
                    return cachedToken;
                }

                var token = await FetchTokenFromExternalApiAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new ApplicationException("Received empty token from external API");
                }

                await _cacheService.SetAsync(CacheKey, token, TimeSpan.FromMinutes(_configuration.GetValue<int>("ExternalAPI:ExpiresInMinutes")));

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get API token");
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


      
    }
}