using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.User
{
    public interface IUserBookingsAndPaymentsApplication
    {
        Task<CreateBookingResponseDTO> CreateBookingAsync(CreateBookingRequestDTO request, int userId);
        Task<CreateBookingPaymentVerificationResponseDTO?> GetBookingForPaymentVerificationAsync(int bookingId, int userId);
        Task<bool> SaveRazorpayOrderAsync(int bookingId, string razorpayOrderId);
        Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm);
        Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel);
        Task<BookingReceiptDto> GetBookingReceiptDetailsAsync(int bookingId);
        Task SaveReceiptAsync(int bookingId, string receiptUrl);
        Task<int> VacateAsync(VacateRequestDTO request);
        Task<List<MyBookingResponseDto>> GetMyBookingsAsync(int userId);
    }
}
