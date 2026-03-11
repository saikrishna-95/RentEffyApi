using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Owner
{
    public interface IGetPostsByOwnerDomain
    {
            Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId);
    }
}
