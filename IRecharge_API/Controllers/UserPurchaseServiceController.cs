using IRecharge_API.BLL;
using IRecharge_API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IRecharge_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPurchaseServiceController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public UserPurchaseServiceController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> UserPurchaseService(
        [FromBody] PurchaseAirtimeRequestDTO request, string username)
        {
            
            var result = await _purchaseService.PurchaseAirtimeService(request, username);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

       
    }
}
