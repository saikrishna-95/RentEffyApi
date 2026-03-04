using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.User
{
    public interface IGetOwnerPostsPersistence
    {
        Task<List<PublicPostResponseDto>> GetPublicPostsAsync();
    }
}
