
using IRecharge_API.BLL;
using IRecharge_API.ExternalServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IRecharge_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirtimeController : ControllerBase
    {
        private readonly AirtimeService _airtimeService;
        private readonly TokenServices _tokenService;

        public AirtimeController(AirtimeService airtimeService, TokenServices tokenService)
        {
            _airtimeService = airtimeService;
            _tokenService = tokenService;
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetApiTokenAsync()
        {
            var token = await _tokenService.GetValidTokenAsync();
            return string.IsNullOrEmpty(token)
                ? StatusCode(500, "Failed to get token")
                : Ok(new { Token = token });
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseAirtime([FromBody] VendAirtimeRequestModel request)
        {
            var result = await _airtimeService.PurchaseAirtime(request);
            return result.isSuccessful
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
