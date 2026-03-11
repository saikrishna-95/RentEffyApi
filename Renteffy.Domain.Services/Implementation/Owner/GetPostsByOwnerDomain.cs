using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.Services.Interfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.Owner
{
    public class GetPostsByOwnerDomain : IGetPostsByOwnerDomain
    {
        private readonly IGetPostsByOwnerPersistance _getPostsByOwnerPersistance;
        public GetPostsByOwnerDomain(IGetPostsByOwnerPersistance getPostsByOwnerPersistance)
        {
            _getPostsByOwnerPersistance = getPostsByOwnerPersistance;
        }
        public async Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId)
        {
            return await _getPostsByOwnerPersistance.GetPostsByOwnerIdAsync(ownerId);
        }
    }
}
