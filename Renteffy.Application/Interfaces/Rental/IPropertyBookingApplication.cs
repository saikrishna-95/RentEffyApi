using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Rental
{
    public interface IPropertyBookingApplication
    {
        Task<CreatePropertyBookingResponseDto> CreateBookingAsync(int userId, CreatePropertyBookingRequestDto request);

        Task<int> ConfirmBookingAsync(int userId, ConfirmPropertyBookingRequestDto request);

        Task<int> CancelBookingAsync(int userId, CancelPropertyBookingRequestDto request);

        Task<int> OwnerCancelBookingAsync(int ownerId, CancelPropertyBookingRequestDto request);

        Task<IEnumerable<PropertyBookingResponseDto>> GetUserBookingsAsync(int userId);

        Task<IEnumerable<PropertyBookingResponseDto>> GetOwnerBookingsAsync(int ownerId);

        Task<PropertyBookingResponseDto?> GetBookingDetailsAsync(int bookingId, int userId);
    }
}
