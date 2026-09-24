using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.Services.Interfaces.Rental;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.Rental
{
    public class GetAllPropertiesDomain : IGetAllPropertiesDomain
    {
        private readonly IGetAllPropertiesPersistance _readRepo;
        private readonly IConfiguration _config;

        public GetAllPropertiesDomain(IGetAllPropertiesPersistance readRepo, IConfiguration config)
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
