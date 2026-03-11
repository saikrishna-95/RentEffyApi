using Azure.Core;
using MediatR;
using Renteffy.Application.Interfaces.Authentication;
using Renteffy.Domain.Services.Interfaces.Authentication;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Application.Implementation.Authetication
{
    public class UserAuthApplication : IUserAuthApplication
    {
        private readonly IUserAuthDomain _domain;
        public UserAuthApplication(IUserAuthDomain domain) => _domain = domain;

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _domain.GetUserByUserNameAsync(request.UserId);
            if (user == null) return null;

            bool valid = await _domain.VerifyPasswordAsync(request.Password, user.PasswordHash);
            if (!valid) return null;

            var roles = await _domain.GetUserRolesAsync(user.UserId);
            var permissions = await _domain.GetUserPermissionsAsync(user.UserId);

            // WEB LOGIN (access token only)
            string token = await _domain.GenerateAccessToken(user);

            if (request.ClientType == "web")
            {
                return new LoginResponseDto
                {
                    AccessToken = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                    Role = roles,
                    Permissions = permissions,
                    UserId = user.UserId,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    FullName = user.FullName
                };
            }

            // MOBILE LOGIN (access + refresh)
            var refreshToken = await _domain.GenerateRefreshToken(user);

            return new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Role = roles,
                Permissions = permissions,
                UserId = user.UserId,
                Email = user.Email,
                Mobile = user.Mobile,
                FullName = user.FullName
            };
        }

        public async Task<LoginResponseDto?> GetRefreshToken(string refreshToken)
        {
            var refreshTokenResponse = await _domain.GetRefreshToken(refreshToken);
            return refreshTokenResponse;
        }
    }
}
