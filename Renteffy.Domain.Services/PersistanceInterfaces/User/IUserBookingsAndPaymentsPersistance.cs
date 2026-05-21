using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.User
{
    public interface IUserBookingsAndPaymentsPersistance
    {
        Task<int> CreateBookingAsync(CreateBookingRequestDTO request);
        Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm);
        Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel);
        Task<BookingReceiptDto>GetBookingReceiptDetailsAsync(int bookingId);
        Task SaveReceiptAsync(int bookingId, string receiptUrl);
        Task<int> VacateAsync(VacateRequestDTO request);
    }
}
