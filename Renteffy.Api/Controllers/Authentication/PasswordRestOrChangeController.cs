using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Application.Interfaces.Authentication;
using Renteffy.Application.Interfaces.PasswordRestChange;
using Renteffy.Shared.Security;

namespace Renteffy.Api.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordRestOrChangeController : ControllerBase
    {
        private readonly IPasswordRestOrChange _service;
        public PasswordRestOrChangeController(IPasswordRestOrChange service) => _service = service;

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
        {
            var result = await _service.GenerateOtpAsync(dto.Mobile);
            return Ok(result); //0- Mobile Not Resister, 1-Otp Send Success
        }

        [AllowAnonymous]
        [HttpPost("ValidateOtp")]
        public async Task<IActionResult> ValidateOtp(ValidateOtpRequestDto dto)
        {
            var result = await _service.ValidateOtpAsync(dto.Mobile, dto.Otp);
            return Ok(result); //0-expired , 1-valid
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Passwords do not match"
                });
            }

            var result = await _service.UpdatePasswordAsync(request);

            if (result <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Unable to reset password"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Password reset successfully"
            });
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (request.NewPassword == request.OldPassword)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "New password cannot be the same as the old password"
                });
            }
            var result = await _service.ChangePassword(request);
            if (result <= 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Unable to change password"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Password changed successfully"
            });
        }
    }
}
