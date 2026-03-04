using Renteffy.Domain.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces
{
    public interface IUserRegistrationPersistence
    {
        Task<int> RegisterUserAsync(Users user);
    }
}
