using Microsoft.Extensions.Configuration;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.User
{
    public class GetOwnerPostsApplication:IGetOwnerPostsApplication
    {
        private readonly IGetOwnerPostsPersistence _readRepo;
        private readonly IConfiguration _config;

        public GetOwnerPostsApplication(IGetOwnerPostsPersistence readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }
        public async Task<List<PublicPostResponseDto>> GetPublicPostsAsync()
            => await _readRepo.GetPublicPostsAsync();
    }
}
