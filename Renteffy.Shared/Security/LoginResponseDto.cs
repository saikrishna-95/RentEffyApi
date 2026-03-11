using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

        public List<string> Role { get; set; } = [];

        public List<string> Permissions { get; set; } = [];
        public int UserId { get; set; }
        public string FullName { get; set; } = default!;

        public string Email { get; set; }= default!;
        public string Mobile { get; set; } = default!;
    }
}
