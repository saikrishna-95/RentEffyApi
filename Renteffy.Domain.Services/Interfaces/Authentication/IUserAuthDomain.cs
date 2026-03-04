using Renteffy.Domain.Entities.Registration;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Authentication
{
    public interface IUserAuthDomain
    {
        Task<Users?> GetUserByUserNameAsync(string userName);
        Task<bool> VerifyPasswordAsync(string password, string passwordHash);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<List<string>> GetUserPermissionsAsync(int userId);

        Task<string> GenerateAccessToken(Users user);
        Task<string> GenerateRefreshToken(Users user);
        Task<LoginResponseDto?> GetRefreshToken(string refreshToken);
    }
}
