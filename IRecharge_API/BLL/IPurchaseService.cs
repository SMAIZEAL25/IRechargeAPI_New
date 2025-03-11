using IRecharge_API.DTO;

namespace IRecharge_API.BLL
{
    public interface IPurchaseService
    {
        ResponseModel PurchaseAirtime(PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO, string username);
       
         ResponseModel PurchaseData(PurchaseDataRequestDTO purchaseDataRequestDTO);
    }
}
