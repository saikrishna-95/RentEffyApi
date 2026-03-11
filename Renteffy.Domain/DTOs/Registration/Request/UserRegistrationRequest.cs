using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.DTOs.Registration.Request
{
    public class UserRegistrationRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
    }
}
