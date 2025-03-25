using IRecharge_API.ExternalServices.Models;

namespace IRecharge_API.ExternalServices
{
    public interface IDigitalVendors
    {
        DigitalVendorsReponseModel VendAirtime(VendAirtimeRequestModel vendAirtimeRequestModel, string token);
    }
}
