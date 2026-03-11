using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class ResetPasswordRequestDto
    {
        public string EmailOrMobile { get; set; } = default!;
        public string NewPassword { get; set; } = default!;

        public string ConfirmPassword { get; set; } = default!;
    }
}
