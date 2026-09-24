using Dapper;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Rental
{
    public class PropertyBookingPersistence : IPropertyBookingPersistence
    {
        private readonly IDbConnectionFactory _dbFactory;
        public PropertyBookingPersistence(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<CreatePropertyBookingResponseDto> CreateBookingAsync(int userId,CreatePropertyBookingRequestDto request)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<CreatePropertyBookingResponseDto>("sp_Property_CreateBooking",
                    new
                    {
                        PropertyID = request.PropertyID,
                        UserID = userId,
                        FromDate = request.FromDate,
                        ToDate = request.ToDate
                    },
                    commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<int> SaveRazorpayOrderAsync(int bookingId,string razorpayOrderId)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync<int>("sp_Property_SaveRazorpayOrder",
                new
                {
                    PropertyBookingID = bookingId,
                    RazorpayOrderID = razorpayOrderId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ConfirmBookingAsync(int userId,ConfirmPropertyBookingRequestDto request)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync<int>("sp_Property_ConfirmBooking",
                new
                {
                    PropertyBookingID = request.PropertyBookingID,
                    RazorpayOrderID = request.RazorpayOrderID,
                    RazorpayPaymentID = request.RazorpayPaymentID,
                    RazorpaySignature = request.RazorpaySignature,
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<dynamic> PrepareUserCancellationAsync(int userId,CancelPropertyBookingRequestDto request)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync("sp_Property_PrepareUserCancellation",
                new
                {
                    PropertyBookingID = request.PropertyBookingID,
                    UserID = userId,
                    Reason = request.Reason
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> FinalizeUserCancellationAsync(int userId,int bookingId,string refundTransactionId)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync<int>("sp_Property_FinalizeUserCancellation",
                new
                {
                    PropertyBookingID = bookingId,
                    UserID = userId,
                    RefundTransactionID = refundTransactionId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RefundFailedAsync(int bookingId,string reason)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync<int>("sp_Property_RefundFailed",
                new
                {
                    PropertyBookingID = bookingId,
                    Reason = reason
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<dynamic> PrepareOwnerCancellationAsync(int ownerId,CancelPropertyBookingRequestDto request)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync("sp_Property_PrepareOwnerCancellation",
                new
                {
                    PropertyBookingID = request.PropertyBookingID,
                    OwnerID = ownerId,
                    Reason = request.Reason
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> FinalizeOwnerCancellationAsync(int ownerId,int bookingId,string refundTransactionId)
        {
            using var con = _dbFactory.CreateConnection();

            return await con.QueryFirstAsync<int>("sp_Property_FinalizeOwnerCancellation",
                new
                {
                    PropertyBookingID = bookingId,
                    OwnerID = ownerId,
                    RefundTransactionID =  refundTransactionId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PropertyBookingResponseDto>> GetUserBookingsAsync(int userId)
        {
            using var con = _dbFactory.CreateConnection();
            return await con.QueryAsync<PropertyBookingResponseDto>("sp_Property_GetUserBookings",
                new
                {
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PropertyBookingResponseDto>> GetOwnerBookingsAsync(int ownerId)
        {
            using var con = _dbFactory.CreateConnection();
            return await con.QueryAsync<PropertyBookingResponseDto>("sp_Property_GetOwnerBookings",
                new
                {
                    OwnerID = ownerId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PropertyBookingResponseDto?> GetBookingDetailsAsync(int bookingId,int userId)
        {
            using var con = _dbFactory.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<PropertyBookingResponseDto>("sp_Property_GetBookingDetails",
                new
                {
                    PropertyBookingID = bookingId,
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
