using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Renteffy.Domain.Services.PersistanceInterfaces.PasswordRestChange;
using Renteffy.Persistence.RegistrationDbContext;
using Renteffy.Shared.Database.DbConnection;
using Renteffy.Shared.Security;
using System.Data;


namespace Renteffy.Persistence.Implementation.PasswordRestChange
{
    public class PasswordRestOrChangePersistance : IPasswordRestOrChangePersistance
    {
        private readonly IDbConnectionFactory _dbFactory;

        public PasswordRestOrChangePersistance(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<PasswordOtpResponseDto> GenerateOtpAsync(string mobile)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstOrDefaultAsync<PasswordOtpResponseDto>(
                "sp_GeneratePasswordOtp",
                new { Mobile = mobile },
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
            {
                return new PasswordOtpResponseDto
                {
                    Success = false,
                    Mobile = "",
                    Email = "",
                };
            }

            bool smsSent = false;
            bool emailSent = false;

            // 📲 Send SMS
            if (!string.IsNullOrWhiteSpace(result.Mobile))
            {
                //smsSent = await _smsService.SendAsync(
                //    otpData.Mobile,
                //    $"Your OTP is {otpData.Otp}"
                //);
                smsSent = true;
            }

            // 📧 Send Email
            if (!string.IsNullOrWhiteSpace(result.Email))
            {
                //emailSent = await _emailService.SendAsync(
                //    otpData.Email,
                //    "Password Reset OTP",
                //    $"Your OTP is {otpData.Otp}"
                //);
                emailSent = true;
            }

            if (!smsSent && !emailSent)
            {
                return new PasswordOtpResponseDto
                {
                    Success = false,
                    Mobile = "",
                    Email = "",
                };
            }

            return new PasswordOtpResponseDto
            {
                Success = true,
                Mobile = smsSent ? MaskMobile(result.Mobile) : "",
                Email = emailSent ? MaskEmail(result.Email) : ""
            };

        }

        private static string MaskMobile(string mobile)
        {
            if (mobile.Length < 4) return "****";
            return $"****{mobile[^4..]}";
        }

        private static string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2) return "****";

            var name = parts[0];
            var domain = parts[1];

            return $"{name[0]}***@{domain}";
        }


        public async Task<int> ValidateOtpAsync(string mobile, string otp)
        {
            using var con = _dbFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Mobile", mobile);
            parameters.Add("@Otp", otp);
            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await con.ExecuteAsync(
                "dbo.sp_ValidatePasswordOtp",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnValue");
        }

        public async Task<int> UpdatePasswordAsync(int userId, string passWordHash)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(passWordHash);

            using var con = _dbFactory.CreateConnection();

            var result =  await con.ExecuteAsync(
                @"UPDATE Users
                SET PasswordHash = @PasswordHash
                WHERE UserId = @UserId AND IsDeleted = 0",
                new { UserId = userId, PasswordHash = password }
            );
            return result;
        }

    }
}
