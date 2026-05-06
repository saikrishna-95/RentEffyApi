using Dapper;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.User
{
    public class UserBookingsAndPaymentsPersistance: IUserBookingsAndPaymentsPersistance
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IRazorpayService _razorpay;
        public UserBookingsAndPaymentsPersistance(IDbConnectionFactory dbFactory, IRazorpayService razorpay)
        {
            _dbFactory = dbFactory;
            _razorpay = razorpay;
        }

        public async Task<int> CreateBookingAsync(CreateBookingRequestDTO request)
        {
            using var con = _dbFactory.CreateConnection();
            var bookingId = await con.QueryFirstAsync<int>("sp_Pg_CreateBooking",
                new {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    FloorId = request.FloorId,
                    RoomId = request.RoomId,
                    BedTypeId = request.BedTypeId,
                    StngPrdId = request.StngPrdId,
                    Price = request.Price,
                    FromDate= request.FromDate,
                    ToDate= request.ToDate
                },
                commandType: CommandType.StoredProcedure
            );

            if (bookingId > 0)
            {
                var order = _razorpay.CreateOrder(request.Price, bookingId.ToString());
            }

            return bookingId;
        }

        public async Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm)
        {
            using var con = _dbFactory.CreateConnection();
            var result = await con.QueryFirstAsync<int>("sp_Pg_ConfirmBooking", 
                new {
                    BookingId = confirm.BookingId,
                    TransactionId = confirm.TransactionId
                },
                commandType: CommandType.StoredProcedure
            );
            return result; // you can return 1 from SP if needed
        }

        public async Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel)
        {
            using var con = _dbFactory.CreateConnection();
            var result = await con.QueryFirstAsync<int>("sp_Pg_CancelBooking",
                new {
                    BookingId = cancel.BookingId,
                    UserId = cancel.UserId
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
}
