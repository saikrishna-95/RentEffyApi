using Microsoft.Extensions.Configuration;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.User
{
    public class UserBookingsAndPaymentsApplication: IUserBookingsAndPaymentsApplication
    {
        private readonly IUserBookingsAndPaymentsDomain _readDomain;
        private readonly IConfiguration _config;

        public UserBookingsAndPaymentsApplication(IUserBookingsAndPaymentsDomain readDomain, IConfiguration config)
        {
            _readDomain = readDomain;
            _config = config;
        }

        public async Task<CreateBookingResponseDTO> CreateBookingAsync(CreateBookingRequestDTO request, int userId)
            => await _readDomain.CreateBookingAsync(request, userId);

        public async Task<CreateBookingPaymentVerificationResponseDTO?> GetBookingForPaymentVerificationAsync(int bookingId, int userId)
            => await _readDomain.GetBookingForPaymentVerificationAsync(bookingId, userId);
        public async Task<bool> SaveRazorpayOrderAsync(int bookingId, string razorpayOrderId)
            => await _readDomain.SaveRazorpayOrderAsync(bookingId, razorpayOrderId);

        public async Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm)
            => await _readDomain.ConfirmBookingAsync(confirm);

        public async Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel)
            => await _readDomain.CancelBookingAsync(cancel);

        public async Task<BookingReceiptDto> GetBookingReceiptDetailsAsync(int bookingId)
            => await _readDomain.GetBookingReceiptDetailsAsync(bookingId);

        public async Task SaveReceiptAsync(int bookingId, string receiptUrl)
            => await _readDomain.SaveReceiptAsync(bookingId, receiptUrl);

        public async Task<int> VacateAsync(VacateRequestDTO request)
            => await _readDomain.VacateAsync(request);

        public async Task<List<MyBookingResponseDto>> GetMyBookingsAsync(int userId)
            => await _readDomain.GetMyBookingsAsync(userId);
    }
}
