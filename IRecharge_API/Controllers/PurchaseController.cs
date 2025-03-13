using IRecharge_API.BLL;
using IRecharge_API.Cache_Management_Service;
using IRecharge_API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IRecharge_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly TokenService _tokenService;

        public PurchaseController(IPurchaseService purchaseService,   TokenService tokenService)
        {
            this._purchaseService = purchaseService;
            this._tokenService = tokenService;
        }


        [HttpPost("purchase-airtime")]
        public IActionResult PurchaseAirtimeEndpoint ([FromBody] PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO)
        {
            // Extract the token from the request headers
            //if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            //{
            //    return Unauthorized("Authorization header is missing");
            //}

            //var token = authHeader.ToString().Replace("Bearer ", ""); // Remove "Bearer " prefix
            /*var username = HttpContext.User.Identity.Name*/
            var username = "JamesSamuel";
            var response = _purchaseService.PurchaseAirtime(purchaseAirtimeRequestDTO, username);
            
            return Ok(response);
        }

        [HttpGet("Get/token")]

        public async Task <IActionResult> GetToken()
        {
            var tokem = await _tokenService.GetTokenAsync();
            return Ok("Token");
        }

    }
}
