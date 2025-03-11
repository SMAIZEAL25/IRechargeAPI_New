using IRecharge_API.BLL;
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

        public PurchaseController(IPurchaseService purchaseService)
        {
            this._purchaseService = purchaseService;
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

    }
}
