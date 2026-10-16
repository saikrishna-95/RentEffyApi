using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.User
{
    public interface IUserBookingsAndPaymentsPersistance
    {
        Task<CreateBookingResponseDTO> CreateBookingAsync(CreateBookingRequestDTO request, int userId);
        Task<bool> SaveRazorpayOrderAsync(int bookingId,string razorpayOrderId);
        Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm);
        Task<CreateBookingPaymentVerificationResponseDTO?>GetBookingForPaymentVerificationAsync(int bookingId,int userId);
        Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel);
        Task<BookingReceiptDto>GetBookingReceiptDetailsAsync(int bookingId);
        Task SaveReceiptAsync(int bookingId, string receiptUrl);
        Task<int> VacateAsync(VacateRequestDTO request);
        Task<List<MyBookingResponseDto>> GetMyBookingsAsync(int userId);
        Task<BookingPaymentDetailsDto?> GetBookingPaymentDetailsAsync(int bookingId);
    }
}
