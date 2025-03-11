using IRecharge_API.DAL;
using IRecharge_API.DTO;
using IRecharge_API.ExternalServices;
using IRecharge_API.ExternalServices.Models;

namespace IRecharge_API.BLL
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDigitalVendors _digitalVendors;

        public PurchaseService(IUserRepository userRepository, IDigitalVendors digitalVendors)
        {
            _userRepository = userRepository;
            _digitalVendors = digitalVendors;
        }
        public ResponseModel PurchaseAirtime(PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username)
        {
            // Check all validation here

            if (purchaseAirtimeRequestDTO == null)
            {
                return new ResponseModel { 
                    Message = "Invalid Request",
                    IsSuccess = false 
                };
            }

            var user = _userRepository.GetByUserName(username);

            if (user == null )
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            if (purchaseAirtimeRequestDTO.Amount > user.WalletBalance)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Insufficient balance"
                };
            }

            user.WalletBalance -= purchaseAirtimeRequestDTO.Amount;
            _userRepository.UpdateUser(user);

            var vendAirtimeRequest = new VendAirtimeRequestModel
            {
                amount = purchaseAirtimeRequestDTO.Amount,
                number = purchaseAirtimeRequestDTO.PhoneNumber,
                network = purchaseAirtimeRequestDTO.NetworkType
            };

            var result = _digitalVendors.VendAirtime(vendAirtimeRequest);

            if (result == null)
            {
                user.WalletBalance += purchaseAirtimeRequestDTO.Amount;
                _userRepository.UpdateUser(user);
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Failed to purchase airtime"
                };
            }

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Airtime purchased successfully"
            };
        }

        public ResponseModel PurchaseData(PurchaseDataRequestDTO purchaseDataRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
