using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Rental
{
    public interface IGetAllPropertiesDomain
    {
        Task<List<PublicPostPropertiesResponseDto>> GetPublicPostPropertiesAsync();
        Task<List<PublicPostPropertiesResponseDto>> GetPropertyByIdAsync(int propertyid);
        Task<List<PublicPostPropertiesResponseDto>> GetPropertiesByOwnerIdAsync(int ownerId);
        Task<EditPropertyResponseDto> GetPropertyForEditAsync(int propertyId);
    }
}
