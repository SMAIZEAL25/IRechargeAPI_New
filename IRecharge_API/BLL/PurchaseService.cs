
using IRecharge_API.DAL;
using IRecharge_API.DTO;
using IRecharge_API.ExternalServices;
using IRecharge_API.ExternalServices.Models;
using System.Net.Http.Headers;

namespace IRecharge_API.BLL
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDigitalVendors _digitalVendors;
        private readonly ILogger<PurchaseService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PurchaseService(
            IUserRepository userRepository,
            IDigitalVendors digitalVendors,
            ILogger<PurchaseService> logger,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _digitalVendors = digitalVendors;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ResponseModel> PurchaseAirtime(
            PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO,
            string username,
            string token)
        {
            try
            {
                _logger.LogInformation($"Starting airtime purchase for user: {username}");

                // Validate request
                if (purchaseAirtimeRequestDTO == null)
                {
                    _logger.LogWarning("Null request received");
                    return new ResponseModel { Message = "Invalid Request", IsSuccess = false };
                }

                // Validate user
                var user = _userRepository.GetByUserName(username);
                if (user == null)
                {
                    _logger.LogWarning($"User not found: {username}");
                    return new ResponseModel { IsSuccess = false, Message = "User not found" };
                }

                // Validate balance
                if (purchaseAirtimeRequestDTO.Amount > user.WalletBalance)
                {
                    _logger.LogWarning($"Insufficient balance for user: {username}");
                    return new ResponseModel { IsSuccess = false, Message = "Insufficient balance" };
                }

                // Deduct balance
                user.WalletBalance -= purchaseAirtimeRequestDTO.Amount;
                 _userRepository.UpdateUserAsync(user);
                _logger.LogDebug($"Deducted {purchaseAirtimeRequestDTO.Amount} from user {username}'s wallet");

                // Prepare vendor request
                _logger.LogInformation($"Preparing airtime purchase request for user: {username}");
                var vendAirtimeRequest = new VendAirtimeRequestModel
                {
                    amount = purchaseAirtimeRequestDTO.Amount,
                    number = purchaseAirtimeRequestDTO.PhoneNumber,
                    network = purchaseAirtimeRequestDTO.NetworkType
                };

                // Add Authorization header to the request
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                // Make the API call
                var response = await _httpClient.PostAsJsonAsync(
                    _configuration["VendorAPI:AirtimeEndpoint"],
                    vendAirtimeRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // Refund if failed
                    user.WalletBalance += purchaseAirtimeRequestDTO.Amount;
                     _userRepository.UpdateUserAsync(user);
                    _logger.LogError($"Airtime purchase failed for user: {username}. Status: {response.StatusCode}");

                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = $"Failed to purchase airtime. Status: {response.StatusCode}"
                    };
                }

                // Process successful response
                var result = await response.Content.ReadFromJsonAsync<ResponseModel>();

                _logger.LogInformation($"Airtime purchase successful for user: {username}");
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Airtime purchased successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing airtime purchase for user: {username}");
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request"
                };
            }
        }

        public async Task<ResponseModel> PurchaseData(PurchaseDataRequestDTO purchaseDataRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}