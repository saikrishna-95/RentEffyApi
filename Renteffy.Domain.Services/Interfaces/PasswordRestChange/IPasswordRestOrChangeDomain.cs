using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.PasswordRestChange
{
    public interface IPasswordRestOrChangeDomain
    {
        Task<PasswordOtpResponseDto> GenerateOtpAsync(string mobile);
        Task<int> ValidateOtpAsync(string mobile, string otp);

        Task<int> UpdatePasswordAsync(int userId, string passwordHash);
    }
}
