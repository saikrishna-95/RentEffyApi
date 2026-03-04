using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Renteffy.Shared.Security
{
    public class PasswordOtpResponseDto
    {
        public string Mobile { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool Success { get; set; } = default!;
        public string Otp { get; set; } = default!;
        
    }
}
