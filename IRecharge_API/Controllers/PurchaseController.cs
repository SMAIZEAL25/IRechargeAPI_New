using IRecharge_API.BLL;
using IRecharge_API.Cache_Management_Service;
using IRecharge_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IRecharge_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly TokenService _tokenService;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(
            IPurchaseService purchaseService,
            TokenService tokenService,
            ILogger<PurchaseController> logger)
        {
            _purchaseService = purchaseService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("purchase-airtime")]
        public async Task<IActionResult> PurchaseAirtime(
            [FromBody] PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO,
            [FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                // Validate token
                var currentToken = await _tokenService.GetTokenAsync();
                if (string.IsNullOrEmpty(token) || token != currentToken)
                {
                    _logger.LogWarning("Invalid or missing token for airtime purchase");
                    return Unauthorized("Invalid or expired token");
                }

                // Get username from authenticated user (better approach)
                var username = User.Identity?.Name ?? "JamesSamuel"; // Fallback for testing

                _logger.LogInformation($"Processing airtime purchase for user: {username}");

                var response = _purchaseService.PurchaseAirtime(purchaseAirtimeRequestDTO, username);

                _logger.LogInformation($"Airtime purchase successful for user: {username}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing airtime purchase");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            try
            {
                var token = await _tokenService.GetTokenAsync();
                _logger.LogDebug("Token generated successfully");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token");
                return StatusCode(500, "Failed to generate token");
            }
        }
    }
}


// Extract the token from the request headers
//if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
//{
//    return Unauthorized("Authorization header is missing");
//}

//var token = authHeader.ToString().Replace("Bearer ", ""); // Remove "Bearer " prefix
/*var username = HttpContext.User.Identity.Name*/
