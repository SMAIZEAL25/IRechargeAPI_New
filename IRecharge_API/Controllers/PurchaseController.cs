using IRecharge_API.BLL;
using IRecharge_API.Cache_Management_Service;
using IRecharge_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace IRecharge_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly TokenService _tokenService;
        private readonly ILogger<PurchaseController> _logger;
        private readonly IConfiguration _configuration;

        public PurchaseController(
            IPurchaseService purchaseService,
            TokenService tokenService,
            ILogger<PurchaseController> logger, IConfiguration configuration)
        {
            _purchaseService = purchaseService;
            _tokenService = tokenService;
            _logger = logger;
            _configuration = configuration;
        }

        //[HttpPost("purchase-airtime")]
        //public async Task<IActionResult> PurchaseAirtime(
        //        [FromBody] PurchaseAirtimeRequestDTO purchaseAirtimeRequestDTO,
        //        [FromHeader(Name = "Authorization")] string token)
        //{
        //    try
        //    {
        //        // Validate token
        //        var currentToken = await _tokenService.GetTokenAsync();
        //        if (string.IsNullOrEmpty(token) || token != currentToken)
        //        {
        //            _logger.LogWarning("Invalid or missing token for airtime purchase");
        //            return Unauthorized("Invalid or expired token");
        //        }

        //        // Get username from authenticated user
        //        var username = User.Identity?.Name ?? "JamesSamuel"; // Fallback for testing

        //        _logger.LogInformation($"Processing airtime purchase for user: {username}");

        //        // Pass the validated token to the service
        //        var response = await _purchaseService.PurchaseAirtime(
        //            purchaseAirtimeRequestDTO,
        //            username,
        //            token);  // Pass the token here

        //        _logger.LogInformation($"Airtime purchase successful for user: {username}");
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing airtime purchase");
        //        return StatusCode(500, "An error occurred while processing your request");
        //    }
        //}

        [HttpPost("purchase-airtime")]
        public async Task<IActionResult> PurchaseAirtime(
        [FromBody] PurchaseAirtimeRequestDTO request,
        [FromHeader(Name = "Authorization")] string authHeader)
        {
            try
            {
                // Validate authorization header
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Invalid authorization header");
                }

                var token = authHeader["Bearer ".Length..].Trim();
                var username = User.Identity?.Name ?? "default_user";

                // Pass the token to the service
                var result = await _purchaseService.PurchaseAirtime(request, username, token);

                return result.IsSuccess
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Purchase failed");
                return StatusCode(500, new { Error = "Processing error" });
            }
        }


        [HttpGet("token")]
     
        public async Task<IActionResult> GetApiTokenAsync()
        {
            try
            {
                var token = await _tokenService.GetApiTokenAsync();
                var expiresIn = _configuration.GetValue<int>("ExternalApi:ExpiresInMinutes");

                _logger.LogInformation("Token generated successfully");

                return Ok(new TokenResponse
                {
                    Token = token,
                    ExpiresInMinutes = expiresIn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token generation failed");
                return StatusCode(500, "Token generation failed");
            }
        }

        //[HttpGet("token")]
        //public async Task<IActionResult> GetToken()
        //{
        //    try
        //    {
        //        var token = await _tokenService.GetApiTokenAsync();
        //        _logger.LogDebug("Token generated successfully");
        //        return Ok(new { Token = token });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error generating token");
        //        return StatusCode(500, "Failed to generate token");
        //    }
        //}
    }
}


