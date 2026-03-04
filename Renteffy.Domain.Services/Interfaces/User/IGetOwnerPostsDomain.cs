using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.User
{
    public interface IGetOwnerPostsDomain
    {
        Task<List<PublicPostResponseDto>> GetPublicPostsAsync();
    }
}
