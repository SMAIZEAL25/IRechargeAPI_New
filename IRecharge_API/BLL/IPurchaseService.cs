using IRecharge_API.DTO;

namespace IRecharge_API.BLL
{
    public interface IPurchaseService
    {
        Task<ResponseModel> PurchaseAirtimeService (PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username);
      
        Task<ResponseModel> PurchaseData (PurchaseDataRequestDTO purchaseDataRequestDTO);
    }
}
