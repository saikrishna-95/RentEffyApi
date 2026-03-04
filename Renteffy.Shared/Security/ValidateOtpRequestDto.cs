using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class ValidateOtpRequestDto
    {
        public string Mobile { get; set; } = default!;
        public string Otp { get; set; } = default!;
    }
}
