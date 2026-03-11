using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Domain.Services.PersistanceInterfaces.PasswordRestChange;
using Renteffy.Persistence.RegistrationDbContext;
using Renteffy.Shared.Database.DbConnection;
using Renteffy.Shared.Security;
using System.Data;
using System.Reflection;
using static System.Net.WebRequestMethods;


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
                Email = emailSent ? MaskEmail(result.Email) : "",
                Otp = result.Otp
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

        public async Task<int> UpdatePasswordAsync(ResetPasswordRequestDto request)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            using var con = _dbFactory.CreateConnection();

            var user = await con.QueryFirstOrDefaultAsync<int?>(
                @"SELECT UserId
                  FROM Users
                  WHERE (Email = @EmailOrMobile OR Mobile = @EmailOrMobile)
                  AND IsDeleted = 0",
                new { EmailOrMobile = request.EmailOrMobile }
            );

            if (user == null)
            {
                return 0;
            }

            var result = await con.ExecuteAsync(
                    @"UPDATE Users
                    SET PasswordHash = @PasswordHash
                    WHERE UserId = @UserId",
                new { PasswordHash = password, UserId = user }
            );

            return result;
        }

        public async Task<int> ChangePassword(ChangePasswordRequest request)
        {
            var oldHash = BCrypt.Net.BCrypt.HashPassword(request.OldPassword);
            var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            using var con = _dbFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@OldPasswordHash", request.OldPassword);
            parameters.Add("@NewPasswordHash", request.NewPassword);
            parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await con.ExecuteAsync("dbo.sp_ChangePassword",parameters,commandType: CommandType.StoredProcedure);
            var result = parameters.Get<int>("@ReturnValue");
            return result;
        }
    }
}
