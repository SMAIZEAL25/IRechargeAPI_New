using IRecharge_API.Cache_Management_Service;
using Newtonsoft.Json;

namespace IRecharge_API.ExternalServices
{
    public class TokenServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TokenServices> _logger;
        private static string _cachedToken;
        private static DateTime _tokenExpiry = DateTime.MinValue;

        public TokenServices(IHttpClientFactory httpClientFactory, ILogger<TokenServices> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> GetValidTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return _cachedToken;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var loginRequest = new
                {
                    username = "fidelis101",
                    password = "12345"
                };

                var response = await client.PostAsJsonAsync(
                    "https://api3.digitalvendorz.com/api/auth/login",
                    loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

                    _cachedToken = tokenResponse?.Token;
                    _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse?.ExpiresInMinutes ?? 86400);

                    return _cachedToken;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get auth token");
            }

            return null;
        }
    }

 
}
