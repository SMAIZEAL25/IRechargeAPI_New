using IRecharge_API.Cache_Management_Service;
using IRecharge_API.DAL;
using IRecharge_API.DTO;
using IRecharge_API.ExternalServices;
using IRecharge_API.ExternalServices.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IRecharge_API.BLL
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDigitalVendors _digitalVendors;
        private readonly TokenService _tokenService;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(
            IUserRepository userRepository,
            IDigitalVendors digitalVendors,
            TokenService tokenService,
            ILogger<PurchaseService> logger)
        {
            _userRepository = userRepository;
            _digitalVendors = digitalVendors;
            _tokenService = tokenService;
            _logger = logger;
        }

        public ResponseModel PurchaseAirtime(PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username)
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
                var vendAirtimeRequest = new VendAirtimeRequestModel
                {
                    amount = purchaseAirtimeRequestDTO.Amount,
                    number = purchaseAirtimeRequestDTO.PhoneNumber,
                    network = purchaseAirtimeRequestDTO.NetworkType
                };

                // Get token asynchronously
                var token = await _tokenService.GetTokenAsync();
                _logger.LogDebug("Retrieved access token");

                // Process airtime purchase
                var result = _digitalVendors.VendAirtime(vendAirtimeRequest, token);

                if (result == null)
                {
                    // Refund if failed
                    user.WalletBalance += purchaseAirtimeRequestDTO.Amount;
                     _userRepository.UpdateUserAsync(user);
                    _logger.LogError($"Airtime purchase failed for user: {username}");

                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Failed to purchase airtime"
                    };
                }

                _logger.LogInformation($"Airtime purchase successful for user: {username}");
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Airtime purchased successfully",
                    Data = result // Include the vendor response if available
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

        public ResponseModel  PurchaseData(PurchaseDataRequestDTO purchaseDataRequestDTO)
        {
            // Similar implementation pattern as PurchaseAirtimeAsync
            throw new NotImplementedException();
        }
    }
}