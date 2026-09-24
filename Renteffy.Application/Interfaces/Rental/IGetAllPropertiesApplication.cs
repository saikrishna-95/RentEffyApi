using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Rental
{
    public interface IGetAllPropertiesApplication
    {
        Task<List<PublicPostPropertiesResponseDto>> GetPublicPostPropertiesAsync();
        Task<List<PublicPostPropertiesResponseDto>> GetPropertyByIdAsync(int propertyid);
        Task<List<PublicPostPropertiesResponseDto>> GetPropertiesByOwnerIdAsync(int ownerId);
        Task<EditPropertyResponseDto> GetPropertyForEditAsync(int propertyId);
    }
}
