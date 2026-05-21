using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Owner
{
    public interface IGetPostsByOwnerDomain
    {
        Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId);
        Task<EditOwnerPostResponseDto> GetPostForEditAsync(int postId);
        Task<List<OwnerBookingResponseDTO>> GetOwnerBookingsAsync(int ownerId);
        Task<int> CheckInAsync(CheckInRequestDTO request);
    }
}
