using Renteffy.Application.DTOs.Registration.Request;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.Interfaces.Registration;
using Renteffy.Domain.Services.PersistanceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.Registration
{
    public class UserRegistrationDomain:IUserRegistrationDomain
    {
        private readonly IUserRegistrationPersistence _persistence;
        public UserRegistrationDomain(IUserRegistrationPersistence persistence)
        {
            _persistence = persistence;
        }
        public async Task<int> RegisterUserAsync(UserRegistrationRequest request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new Users
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = passwordHash
            };

            return await _persistence.RegisterUserAsync(user);
        }
    }
}
