using Renteffy.Application.DTOs.Registration.Request;
using Renteffy.Application.Interfaces.Registration;
using Renteffy.Domain.Services.Interfaces.Registration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.Registration
{
    public class UserRegistrationApplication:IUserRegistrationAapplication
    {
        private readonly IUserRegistrationDomain _userRegistrationDomain;
        public UserRegistrationApplication(IUserRegistrationDomain userRegistrationDomain)
        {
            _userRegistrationDomain = userRegistrationDomain;
        }
        public async Task<int> RegisterUserAsync(UserRegistrationRequest request)
        {
            return await _userRegistrationDomain.RegisterUserAsync(request);
        }
    }
}
