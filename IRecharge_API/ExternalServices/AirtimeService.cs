using Newtonsoft.Json;
using System.Text;

namespace IRecharge_API.ExternalServices.Models
{
    public class AirtimeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenServices _tokenService;
        private readonly ILogger<AirtimeService> _logger;

        public AirtimeService(
            IHttpClientFactory httpClientFactory,
            TokenServices tokenService,
            ILogger<AirtimeService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<DigitalVendorsReponseModel> PurchaseAirtime(VendAirtimeRequestModel requestModel)
        {
            var responseModel = new DigitalVendorsReponseModel();

            try
            {
                // Get valid token
                var authToken = await _tokenService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(authToken))
                {
                    responseModel.isSuccessful = false;
                    responseModel.responsemessage = "Failed to obtain authentication token";
                    return responseModel;
                }

                // Create request
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api3.digitalvendorz.com/api/airtime");

                request.Headers.Add("Auth", authToken);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(requestModel),
                    Encoding.UTF8,
                    "application/json");

                // Send request
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                // Handle response
                if (!response.IsSuccessStatusCode)
                {
                    responseModel.isSuccessful = false;
                    responseModel.responsemessage = $"API request failed ({(int)response.StatusCode}): {responseString}";
                    return responseModel;
                }

                var apiResponse = JsonConvert.DeserializeObject<DigitalVendorsReponseModel>(responseString);
                return apiResponse ?? responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Airtime purchase failed");
                responseModel.isSuccessful = false;
                responseModel.responsemessage = ex.Message;
                return responseModel;
            }
        }
    }
}
