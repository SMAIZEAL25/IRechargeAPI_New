using IRecharge_API.ExternalServices.Models;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace IRecharge_API.ExternalServices
{
    public class DigitalVendorsAPI : IDigitalVendors
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public DigitalVendorsAPI(IConfiguration configuration, HttpClient httpClient)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
        }

        public async Task <DigitalVendorsReponseModel> VendAirtime(VendAirtimeRequestModel vendAirtimeRequestModel, string token)
        {
            var baseurl = _configuration.GetSection("DigitalVendorsAPI:BaseURL").Value;
            if (string.IsNullOrEmpty(baseurl))
            {
                return null;
            }

            var jsonContent = JsonSerializer.Serialize(vendAirtimeRequestModel);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.PostAsync(baseurl, httpContent).Result;

            // Log the response status and content
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                return new DigitalVendorsReponseModel
                {
                    responsemessage = "Failed to purchase airtime",
                    isSuccessful = false
                };
            }

            var deserializedResponse = JsonSerializer.Deserialize<DigitalVendorsReponseModel>(responseContent);

            return deserializedResponse ?? new DigitalVendorsReponseModel();
        }
    }
}