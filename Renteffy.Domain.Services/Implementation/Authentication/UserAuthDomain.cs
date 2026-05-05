using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.Interfaces.Authentication;
using Renteffy.Domain.Services.PersistanceInterfaces.Authentication;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Renteffy.Domain.Services.Implementation.Authentication
{
    public class UserAuthDomain:IUserAuthDomain
    {
        private readonly IUserReadPersistance _readRepo;
        private readonly IConfiguration _config;

        public UserAuthDomain(IUserReadPersistance readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }

        public async Task<UserProfileResponseDto> GetUserProfile(int userId)
            => await _readRepo.GetUserProfile(userId);

        public async Task<UpdateUserProfileResponse2Dto> UpdateUserProfile(UpdateUserProfileResponseDto model)
             => await _readRepo.UpdateUserProfile(model);

        public async Task<Users?> GetUserByUserIdAsync(int userId)
            => await _readRepo.GetUserByUserIdAsync(userId);

        public async Task<Users?> GetUserByUserNameAsync(string userName)
            => await _readRepo.GetUserByUserNameAsync(userName);

        public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
                    => Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordHash));

        public async Task<List<string>> GetUserRolesAsync(int userId)
                => await _readRepo.GetUserRolesAsync(userId);

        public async Task<List<string>> GetUserPermissionsAsync(int userId)
                => await _readRepo.GetUserPermissionsAsync(userId);


        public async Task<string> GenerateAccessToken(Users user)
        {
            var minutes = int.Parse(_config["JwtSettings:ExpiryMinutes"]!);
            return await GenerateJwtToken(user, "access", DateTime.UtcNow.AddMinutes(minutes));
        }

        public async Task<string> GenerateRefreshToken(Users user)
        {
            var days = int.Parse(_config["JwtSettings:RefreshTokenDays"]!);
            return await GenerateJwtToken(user, "refresh", DateTime.UtcNow.AddDays(days));
        }

        public async Task<string> GenerateJwtToken(Users user, string tokenType,DateTime expiresAt)
        {
            var jwt = _config.GetSection("JwtSettings");
            var secretKey = jwt["SecretKey"]!;
            var issuer = jwt["Issuer"]!;
            var audience = jwt["Audience"]!;
            var expiry = int.Parse(jwt["ExpiryMinutes"]!);
            var refreshexpiry = int.Parse(jwt["RefreshTokenDays"]!);

            var key = Encoding.UTF8.GetBytes(secretKey);

            var roles = await GetUserRolesAsync(user.UserId);
            var permissions = await GetUserPermissionsAsync(user.UserId);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("Email", user.Email.ToString()),
                new Claim("Mobile", user.Mobile),
                new Claim("type", tokenType)
            };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            permissions.ForEach(permission => claims.Add(new Claim("permission", permission)));

            var token = new JwtSecurityToken(
                issuer, audience, claims,
                //expires: DateTime.UtcNow.AddMinutes(expiry),
                expires: expiresAt,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponseDto?> GetRefreshToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwt = _config.GetSection("JwtSettings");
                var secretKey = jwt["SecretKey"]!;
                var issuer = jwt["Issuer"]!;
                var audience = jwt["Audience"]!;
                var expiry = int.Parse(jwt["ExpiryMinutes"]!);
                var refreshexpiry = int.Parse(jwt["RefreshTokenDays"]!);

                var key = Encoding.UTF8.GetBytes(secretKey);
               
                var principal = handler.ValidateToken(
                    refreshToken,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero // 🔑 important (no extra 5 min delay)
                    },
                    out _
                );

                // 🔐 Ensure it's a refresh token
                var tokenType = principal.FindFirst("type")?.Value;
                if (tokenType != "refresh")
                    return null;

                var userIdClaim = principal.FindFirst("UserId")?.Value;
                if (userIdClaim == null)
                    return null;

                var user = await GetUserByUserIdAsync(int.Parse(userIdClaim));
                if (user == null)
                    return null;

                var newAccessToken = await GenerateAccessToken(user);
                var newRefreshToken =await GenerateRefreshToken(user);

                var roles = await GetUserRolesAsync(user.UserId);
                var permissions = await GetUserPermissionsAsync(user.UserId);

                return new LoginResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(expiry),
                    Role = roles,
                    Permissions = permissions,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    FullName = user.FullName
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
