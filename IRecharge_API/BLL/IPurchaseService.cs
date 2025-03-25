using IRecharge_API.DTO;

namespace IRecharge_API.BLL
{
    public interface IPurchaseService
    {
        Task<ResponseModel> PurchaseAirtime (PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username);
        Task<ResponseModel> PurchaseData (PurchaseDataRequestDTO purchaseDataRequestDTO);
    }
}
