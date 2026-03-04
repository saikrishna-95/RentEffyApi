using Renteffy.Application.DTOs.Registration.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Registration
{
    public interface IUserRegistrationAapplication
    {
        Task<int> RegisterUserAsync(UserRegistrationRequest request);
    }
}
