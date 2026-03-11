using Renteffy.Application.Interfaces.Owner;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.Services.Interfaces.Owner;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.Owner
{
    public class GetPostsByOwnerApplication:IGetPostsByOwnerApplication
    {
        private readonly IGetPostsByOwnerDomain _getPostsByOwnerDomain;
        public GetPostsByOwnerApplication(IGetPostsByOwnerDomain getPostsByOwnerDomain)
        {
            _getPostsByOwnerDomain = getPostsByOwnerDomain;
        }
        public async Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId)
        {
            return await _getPostsByOwnerDomain.GetPostsByOwnerIdAsync(ownerId);
        }
    }
}
