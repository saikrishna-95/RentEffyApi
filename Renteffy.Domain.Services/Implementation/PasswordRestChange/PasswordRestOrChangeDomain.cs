using Microsoft.Extensions.Configuration;
using Renteffy.Domain.Services.Interfaces.PasswordRestChange;
using Renteffy.Domain.Services.PersistanceInterfaces.Authentication;
using Renteffy.Domain.Services.PersistanceInterfaces.PasswordRestChange;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.PasswordRestChange
{
    public class PasswordRestOrChangeDomain : IPasswordRestOrChangeDomain
    {
        private readonly IPasswordRestOrChangePersistance _readRepo;
        private readonly IConfiguration _config;

        public PasswordRestOrChangeDomain(IPasswordRestOrChangePersistance readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }
        public async Task<PasswordOtpResponseDto> GenerateOtpAsync(string mobile)
        {
            var result = await _readRepo.GenerateOtpAsync(mobile);
            return result;
        }

        public async Task<int> ValidateOtpAsync(string mobile,string otp)
        {
            var result = await _readRepo.ValidateOtpAsync(mobile,otp);
            return result;
        }

        public async Task<int> UpdatePasswordAsync(ResetPasswordRequestDto request)
        {
            var result = await _readRepo.UpdatePasswordAsync(request);
            return result;
        }
        public async Task<int> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _readRepo.ChangePassword(request);
            return result;
        }
    }
}
