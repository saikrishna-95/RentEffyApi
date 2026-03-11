using Azure.Core;
using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.Interfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.Authentication;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.Owner
{
    public class AddPostDomain:IAddPostDomain
    {
        private readonly IAddPostPersistence _readRepo;
        private readonly IConfiguration _config;

        public AddPostDomain(IAddPostPersistence readRepo, IConfiguration config)
        {

            _readRepo = readRepo;
            _config = config;
        }
        public async Task<int> AddPostAsync(AddPostRequestDto request)
            => await _readRepo.AddPostAsync(request);
    }
}
