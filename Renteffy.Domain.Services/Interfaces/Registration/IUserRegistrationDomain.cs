using Renteffy.Application.DTOs.Registration.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Registration
{
    public interface IUserRegistrationDomain
    {
        Task<int> RegisterUserAsync(UserRegistrationRequest request);
    }
}
