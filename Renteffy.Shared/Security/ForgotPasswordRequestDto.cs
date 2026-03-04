using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class ForgotPasswordRequestDto
    {
        public string UserName { get; set; } = default;
        public string Mobile { get; set; } = default!;
    }
}
