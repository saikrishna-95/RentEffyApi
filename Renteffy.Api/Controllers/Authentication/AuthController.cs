using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Application.Interfaces.Authentication;
using Renteffy.Application.Interfaces.Registration;
using Renteffy.Domain.Services.Interfaces.Authentication;
using Renteffy.Shared.Security;
using System.Threading.Tasks;

namespace Renteffy.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthApplication _service;
        public AuthController(IUserAuthApplication service) => _service = service;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var loginResult = await _service.LoginAsync(request);

            if (loginResult == null)
                return Unauthorized(new { Success = false, Message = "Invalid credentials" });

            return Ok(new { Success = true, Data = loginResult });
        }

        [AllowAnonymous]
        [HttpPost("GetRefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _service.GetRefreshToken(request.RefreshToken);

            if (result == null)
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid or expired refresh token"
                });

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }
    }
}
