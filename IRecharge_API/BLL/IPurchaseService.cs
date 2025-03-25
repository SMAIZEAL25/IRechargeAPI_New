using IRecharge_API.DTO;

namespace IRecharge_API.BLL
{
    public interface IPurchaseService
    {
        Task<ResponseModel> PurchaseAirtime (PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username, string token);
      
        Task<ResponseModel> PurchaseData (PurchaseDataRequestDTO purchaseDataRequestDTO);
    }
}
