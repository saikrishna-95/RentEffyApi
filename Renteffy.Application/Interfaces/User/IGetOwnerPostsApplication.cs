using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.User
{
    public interface IGetOwnerPostsApplication
    {
        Task<List<PublicPostResponseDto>> GetPublicPostsAsync();
    }
}
