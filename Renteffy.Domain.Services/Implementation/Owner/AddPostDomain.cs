using Azure.Core;
using Microsoft.AspNetCore.Http;
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

        public async Task<int> UpdatePostAsync(UpdatePostRequestDTO request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles)
            => await _readRepo.UpdatePostAsync(request, replaceMedia, newFiles);
        public async Task<bool> DeletePostAsync(int postId, int userId)
            => await _readRepo.DeletePostAsync(postId, userId);

        public async Task<bool> UpdatePostStatusAsync(int postId, int userId, int status)
            => await _readRepo.UpdatePostStatusAsync(postId, userId, status);
    }
}
