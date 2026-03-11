using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
