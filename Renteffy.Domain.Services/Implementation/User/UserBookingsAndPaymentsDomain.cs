using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.User
{
    public class UserBookingsAndPaymentsDomain:IUserBookingsAndPaymentsDomain
    {
        private readonly IUserBookingsAndPaymentsPersistance _readRepo;
        private readonly IConfiguration _config;

        public UserBookingsAndPaymentsDomain(IUserBookingsAndPaymentsPersistance readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }

        public async Task<CreateBookingResponseDTO> CreateBookingAsync(CreateBookingRequestDTO request, int userId)
            => await _readRepo.CreateBookingAsync(request, userId);

        public async Task<CreateBookingPaymentVerificationResponseDTO?> GetBookingForPaymentVerificationAsync(int bookingId, int userId)
            => await _readRepo.GetBookingForPaymentVerificationAsync(bookingId, userId);

        public async Task<bool> SaveRazorpayOrderAsync(int bookingId,string razorpayOrderId)
          => await _readRepo.SaveRazorpayOrderAsync(bookingId,razorpayOrderId);
        

        public async Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm)
            => await _readRepo.ConfirmBookingAsync(confirm);

        public async Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel)
            => await _readRepo.CancelBookingAsync(cancel);

        public async Task<BookingReceiptDto> GetBookingReceiptDetailsAsync(int bookingId)
            => await _readRepo.GetBookingReceiptDetailsAsync(bookingId);

        public async Task SaveReceiptAsync(int bookingId, string receiptUrl)
            => await _readRepo.SaveReceiptAsync(bookingId, receiptUrl);

        public async Task<int> VacateAsync(VacateRequestDTO request)
            => await _readRepo.VacateAsync(request);

        public async Task<List<MyBookingResponseDto>> GetMyBookingsAsync(int userId)
            => await _readRepo.GetMyBookingsAsync(userId);
    }
}
