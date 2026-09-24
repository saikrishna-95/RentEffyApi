using Microsoft.Extensions.Configuration;
using Renteffy.Application.Interfaces.Rental;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.Services.Interfaces.Rental;
using Renteffy.Domain.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.Rental
{
    public class GetAllPropertiesApplication : IGetAllPropertiesApplication
    {
        private readonly IGetAllPropertiesDomain _readRepo;
        private readonly IConfiguration _config;

        public GetAllPropertiesApplication(IGetAllPropertiesDomain readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }

        public async Task<List<PublicPostPropertiesResponseDto>> GetPublicPostPropertiesAsync()
            => await _readRepo.GetPublicPostPropertiesAsync();
        public async Task<List<PublicPostPropertiesResponseDto>> GetPropertyByIdAsync(int propertyid)
                => await _readRepo.GetPropertyByIdAsync(propertyid);
        public async Task<List<PublicPostPropertiesResponseDto>> GetPropertiesByOwnerIdAsync(int ownerId)
            => await _readRepo.GetPropertiesByOwnerIdAsync(ownerId);
        public async Task<EditPropertyResponseDto> GetPropertyForEditAsync(int propertyId)
            => await _readRepo.GetPropertyForEditAsync(propertyId);
    }
}
