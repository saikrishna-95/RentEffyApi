using Renteffy.Application.Interfaces.PasswordRestChange;
using Renteffy.Domain.Services.Interfaces.Authentication;
using Renteffy.Domain.Services.Interfaces.PasswordRestChange;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.PasswordRestChange
{

    public class PasswordRestOrChange : IPasswordRestOrChange
    {
        private readonly IPasswordRestOrChangeDomain _domain;
        public PasswordRestOrChange(IPasswordRestOrChangeDomain domain) => _domain = domain;
        public async Task<PasswordOtpResponseDto> GenerateOtpAsync(string mobile)
        {
            var result = await _domain.GenerateOtpAsync(mobile);
            return result;  //0- mobile not registered.
        }

        public async Task<int> ValidateOtpAsync(string mobile, string otp)
        {
            var result = await _domain.ValidateOtpAsync(mobile, otp);
            return result;
        }

        public async Task<int> UpdatePasswordAsync(int userId, string passwordHash)
        {
            var result = await _domain.UpdatePasswordAsync(userId, passwordHash);
            return result;
        }
    }
}
