using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Owner
{
    public interface IGetPostsByOwnerApplication
    {
        Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId);
    }
}
