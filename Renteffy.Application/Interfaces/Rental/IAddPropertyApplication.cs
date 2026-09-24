using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.UserTrans.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Rental
{
    public interface IAddPropertyApplication
    {
        Task<int> AddProperty(AddPropertyRequestDto request, List<IFormFile> files, List<PropertyMediaDto> mediaMeta);
        Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles);
        Task<bool> UpdatePropertyStatusAsync(int propertyId, int userId, int status);
        Task<bool> DeletePropertyAsync(int propertyId, int userId);
    }
}
