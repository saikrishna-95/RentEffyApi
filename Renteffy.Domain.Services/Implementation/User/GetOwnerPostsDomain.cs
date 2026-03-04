using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.User
{
    public class GetOwnerPostsDomain:IGetOwnerPostsDomain
    {
        private readonly IGetOwnerPostsPersistence _readRepo;
        private readonly IConfiguration _config;

        public GetOwnerPostsDomain(IGetOwnerPostsPersistence readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }
        public async Task<List<PublicPostResponseDto>> GetPublicPostsAsync()
            => await _readRepo.GetPublicPostsAsync();
        
    }
}
