using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Owner
{
    public interface IGetPostsByOwnerPersistance
    {
        Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId);
    }
}
