using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Rental
{
    public interface IPropertyBookingPersistence
    {
        Task<CreatePropertyBookingResponseDto> CreateBookingAsync(int userId,CreatePropertyBookingRequestDto request);
        Task<int> ConfirmBookingAsync(int userId,ConfirmPropertyBookingRequestDto request);
        Task<int> SaveRazorpayOrderAsync(int bookingId,string razorpayOrderId);
        Task<dynamic>PrepareUserCancellationAsync(int userId,CancelPropertyBookingRequestDto request);
        Task<int> FinalizeUserCancellationAsync(int userId,int bookingId,string refundTransactionId);
        Task<int> RefundFailedAsync(int bookingId,string reason);
        Task<dynamic> PrepareOwnerCancellationAsync(int ownerId,CancelPropertyBookingRequestDto request);
        Task<int> FinalizeOwnerCancellationAsync(int ownerId,int bookingId,string refundTransactionId);
        Task<IEnumerable<PropertyBookingResponseDto>> GetUserBookingsAsync(int userId);
        Task<IEnumerable<PropertyBookingResponseDto>> GetOwnerBookingsAsync(int ownerId);
        Task<PropertyBookingResponseDto?> GetBookingDetailsAsync(int bookingId,int userId);
    }
}
