using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Application.DTOs.Registration.Request;
using Renteffy.Application.Interfaces.Registration;
using Renteffy.Shared.Response;

namespace Renteffy.Api.Controllers.Registartion
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationAapplication _userRegistrationAapplication;
        public UserRegistrationController(IUserRegistrationAapplication userRegistrationAapplication)
        {
            _userRegistrationAapplication = userRegistrationAapplication;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UserRegister")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRegistrationRequest request)
        {
            var result = await _userRegistrationAapplication.RegisterUserAsync(request);

            if (result == 1)
            {
                return Ok(new ApiResponse<string> { Success = true, Message = "User registered successfully" });
            }
            else if (result == 2)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Already MobileNumber Existed." });
            }
            else if (result == 0)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Already Registered." });
            }
            else
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Registration failed" });
            }
        }
    }
}
