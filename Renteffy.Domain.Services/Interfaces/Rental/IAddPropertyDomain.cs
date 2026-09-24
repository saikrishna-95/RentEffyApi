using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Rental
{
    public interface IAddPropertyDomain
    {
        Task<int> AddProperty(AddPropertyRequestDto request);
        Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles);

        Task<bool> UpdatePropertyStatusAsync(int propertyId, int userId, int status);
        Task<bool> DeletePropertyAsync(int propertyId, int userId);
    }
}
