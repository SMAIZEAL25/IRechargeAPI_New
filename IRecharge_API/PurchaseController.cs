//using IRecharge_API.BLL;
//using IRecharge_API.DTO;
//using IRecharge_API.Entities;
//using IRecharge_API.ExternalServices;
//using IRecharge_API.ExternalServices.Models;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System.Text;


//namespace IRecharge_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PurchaseController : ControllerBase
//    {
//        private readonly IPurchaseService _purchaseService;
//        private readonly TokenServices _tokenService;
//        private readonly ILogger<PurchaseController> _logger;
//        private readonly IConfiguration _configuration;

//        public PurchaseController(
//            IPurchaseService purchaseService,
//            TokenServices tokenService,
//            ILogger<PurchaseController> logger, IConfiguration configuration)
//        {
//            _purchaseService = purchaseService;
//            _tokenService = tokenService;
//            _logger = logger;
//            _configuration = configuration;
//        }

//        //[HttpPost("purchase-airtime")]
//        //public async Task<IActionResult> PurchaseAirtime(
//        //        [FromBody] PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO,
//        //        [FromHeader(Name = "Authorization")] string token)
//        //{
//        //    try
//        //    {
//        //        // Validate token
//        //        var currentToken = await _tokenService.GetTokenAsync();
//        //        if (string.IsNullOrEmpty(token) || token != currentToken)
//        //        {
//        //            _logger.LogWarning("Invalid or missing token for airtime purchase");
//        //            return Unauthorized("Invalid or expired token");
//        //        }

//        //        // Get username from authenticated user
//        //        var username = User.Identity?.Name ?? "JamesSamuel"; // Fallback for testing

//        //        _logger.LogInformation($"Processing airtime purchase for user: {username}");

//        //        // Pass the validated token to the service
//        //        var response = await _purchaseService.PurchaseAirtime(
//        //            purchaseAirtimeRequestDTO,
//        //            username,
//        //            token);  // Pass the token here

//        //        _logger.LogInformation($"Airtime purchase successful for user: {username}");
//        //        return Ok(response);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError(ex, "Error processing airtime purchase");
//        //        return StatusCode(500, "An error occurred while processing your request");
//        //    }
//        //}

//        [HttpPost("purchase-airtime")]
//        public async Task<DigitalVendorsReponseModel> PurchaseAirtime(VendAirtimeRequestModel vendAirtimeRequestModel)
  
//        {
//            var responseModel = new DigitalVendorsReponseModel();
//            try
//            {

//                var client = new HttpClient();
//                var request1 = new HttpRequestMessage(HttpMethod.Post, "https://api3.digitalvendorz.com/api/airtime");
//                //request1.Headers.Add("Auth", "10|hKOM1xo4wtRorExk14vIQA9jN6o00CHJOgo3YAAe");
//                request1.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
//                ("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczpcL1wvYXBpMy5kaWdpdGFsdmVuZG9yei5jb21cL2FwaVwvYXV0aFwvbG9naW4iLCJpYXQiOjE3NDM0OTYyMTgsImV4cCI6MTc0MzU4MjYxOCwibmJmIjoxNzQzNDk2MjE4LCJqdGkiOiJ3ZzVPb2t1a0tKZ3k0TFkyIiwic3ViIjoxLCJwcnYiOiIyM2JkNWM4OTQ5ZjYwMGFkYjM5ZTcwMWM0MDA4NzJkYjdhNTk3NmY3In0.tfRX2bN8zuzMfMH6ij9RVQcDpLj-gGO3KC0yC9kfuTw");
//                var jsonContent = JsonConvert.SerializeObject(vendAirtimeRequestModel);
//                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
   
//                var response = await client.SendAsync(request1);
//                var Jsonresponse = await response.Content.ReadAsStringAsync();
//                var contentType = response.Content.Headers.ContentType?.MediaType;
//                if (contentType == "text/html")
//                {
//                    responseModel.isSuccessful = false;
//                    responseModel.responsemessage = "Authentication failed - possibly expired token";

//                    return responseModel; 
//                }

//                var apiResponse = JsonConvert.DeserializeObject<DigitalVendorsReponseModel>(contentType);

//                return apiResponse;

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Purchase failed");
//                responseModel.isSuccessful = false;
//                responseModel.responsemessage = "Processing error";
//                return responseModel;
//            }
//        }


//        [HttpGet("token")]

//        public async Task<IActionResult> GetApiTokenAsync()
//        {

//            var client = new HttpClient();
//            var request = new HttpRequestMessage(HttpMethod.Post, "https://api3.digitalvendorz.com/api/auth/login");
//            var content = new StringContent("{\r\n    \"username\":\"fidelis101\",\r\n    \"password\":\"12345\"\r\n}", null, "application/json");
//            request.Content = content;
//            var response = await client.SendAsync(request);
//            //response.EnsureSuccessStatusCode();
//            var test = await response.Content.ReadAsStringAsync();

//            var jsonresponse = JsonConvert.DeserializeObject<TokenResponses>(test);

//            return Ok(new  {
//                Token = jsonresponse.token,
//                token_validity = jsonresponse.token_validity
//            });
//        }
//    }
//}


