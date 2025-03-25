using IRecharge_API.ExternalServices.Models;

namespace IRecharge_API.ExternalServices
{
    public interface IDigitalVendors
    {
        Task <DigitalVendorsReponseModel> VendAirtime(VendAirtimeRequestModel vendAirtimeRequestModel, string token);
    }
}
