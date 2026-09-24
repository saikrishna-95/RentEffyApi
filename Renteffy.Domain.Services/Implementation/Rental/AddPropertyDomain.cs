using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.Services.Interfaces.Rental;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.Rental
{
    public class AddPropertyDomain:IAddPropertyDomain
    {
        private readonly IAddPostPropertyPersistence _readRepo;
        private readonly IConfiguration _config;

        public AddPropertyDomain(IAddPostPropertyPersistence readRepo, IConfiguration config)
        {

            _readRepo = readRepo;
            _config = config;
        }
        public async Task<int> AddProperty(AddPropertyRequestDto request)
            => await _readRepo.AddProperty(request);

        public async Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles)
            => await _readRepo.UpdatePropertyAsync(request, replaceMedia, newFiles);

        public async Task<bool> UpdatePropertyStatusAsync(int propertyId, int userId, int status)
            => await _readRepo.UpdatePropertyStatusAsync(propertyId, userId, status);

        public async Task<bool> DeletePropertyAsync(int propertyId, int userId)
            => await _readRepo.DeletePropertyAsync(propertyId, userId);
    }
}
