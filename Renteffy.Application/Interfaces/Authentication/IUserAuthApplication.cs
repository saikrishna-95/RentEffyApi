using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Authentication
{
    public interface IUserAuthApplication
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto?> GetRefreshToken(string refreshToken);
    }
}
