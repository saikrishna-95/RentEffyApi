using Microsoft.Extensions.Configuration;
using Renteffy.Application.Interfaces.Rental;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Renteffy.Application.Implementation.Rental
{
    public class PropertyBookingApplication : IPropertyBookingApplication
    {
        private readonly IPropertyBookingPersistence _persistence;
        private readonly IRazorpayService _razorpay;
        private readonly IConfiguration _configuration;
        public PropertyBookingApplication(IPropertyBookingPersistence persistence,IRazorpayService razorpay,IConfiguration configuration)
        {
            _persistence = persistence;
            _razorpay = razorpay;
            _configuration = configuration;
        }

        public async Task<CreatePropertyBookingResponseDto> CreateBookingAsync(int userId,CreatePropertyBookingRequestDto request)
        {
            var booking = await _persistence.CreateBookingAsync(userId,request);
            var order = _razorpay.CreateOrder(booking.TotalAmount,booking.PropertyBookingID.ToString());
            var razorpayOrderId = order["id"]?.ToString();
            if (string.IsNullOrWhiteSpace(razorpayOrderId))
            {
                throw new Exception("Unable to create Razorpay order.");
            }
            var saved = await _persistence.SaveRazorpayOrderAsync(booking.PropertyBookingID,razorpayOrderId);
            if (saved != 1)
            {
                throw new Exception("Unable to save Razorpay order.");
            }
            return new CreatePropertyBookingResponseDto
            {
                PropertyBookingID = booking.PropertyBookingID,
                TotalAmount = booking.TotalAmount,
                RazorpayOrderID = razorpayOrderId,
                Amount = booking.TotalAmount,
                Currency = "INR",
                RazorpayKey = _configuration["Razorpay:Key"]!
            };
        }

        public async Task<int>ConfirmBookingAsync(int userId,ConfirmPropertyBookingRequestDto request)
        {
            return await _persistence.ConfirmBookingAsync(userId,request);
        }

        public async Task<int> CancelBookingAsync(int userId,CancelPropertyBookingRequestDto request)
        {
            var cancellation = await _persistence.PrepareUserCancellationAsync(userId,request);
            string refundId;

            try
            {
                refundId = _razorpay.CreateRefund(cancellation.RazorpayPaymentID,cancellation.RefundAmount,request.Reason);
            }
            catch (Exception ex)
            {
                await _persistence.RefundFailedAsync(request.PropertyBookingID,ex.Message);
                throw;
            }

            return await _persistence.FinalizeUserCancellationAsync(userId,request.PropertyBookingID,refundId);
        }

        public async Task<int> OwnerCancelBookingAsync(int ownerId,CancelPropertyBookingRequestDto request)
        {
            var cancellation = await _persistence.PrepareOwnerCancellationAsync(ownerId,request);
            string refundId;

            try
            {
                refundId = _razorpay.CreateRefund(cancellation.RazorpayPaymentID,cancellation.RefundAmount,request.Reason);
            }
            catch (Exception ex)
            {
                await _persistence.RefundFailedAsync(request.PropertyBookingID,ex.Message);
                throw;
            }

            return await _persistence.FinalizeUserCancellationAsync(ownerId,request.PropertyBookingID,refundId);
        }

        public async Task<IEnumerable<PropertyBookingResponseDto>> GetUserBookingsAsync(int userId)
        {
            return await _persistence.GetUserBookingsAsync(userId);
        }

        public async Task<IEnumerable<PropertyBookingResponseDto>> GetOwnerBookingsAsync(int ownerId)
        {
            return await _persistence.GetOwnerBookingsAsync(ownerId);
        }

        public async Task<PropertyBookingResponseDto?>GetBookingDetailsAsync(int bookingId,int userId)
        {
            return await _persistence.GetBookingDetailsAsync(bookingId,userId);
        }
    }
}
