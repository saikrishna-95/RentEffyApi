using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Rental
{
    public interface IAddPostPropertyPersistence
    {
        Task<int> AddProperty(AddPropertyRequestDto request);
        Task SavePropertyMediaAsync(List<PropertyPostMediaDto> media);
        Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request,List<(int mediaId, IFormFile file)> replaceMedia,List<IFormFile> newFiles);
        Task<bool> UpdatePropertyStatusAsync(int propertyId,int userId,int status);
        Task<bool> DeletePropertyAsync(int propertyId, int userId);
    }
}
