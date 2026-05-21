using Dapper;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
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
                    BedId = request.BedId,
                    StngPrdId = request.StngPrdId,
                    Price = request.Price,
                    FromDate= request.FromDate,
                    ToDate= request.ToDate
                },
                commandType: CommandType.StoredProcedure
            );

            //if (bookingId > 0)
            //{
            //    var order = _razorpay.CreateOrder(request.Price, bookingId.ToString());
            //}

            return bookingId;
        }

        public async Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm)
        {
            using var con = _dbFactory.CreateConnection();
            var result = await con.QueryFirstAsync<int>("sp_Pg_ConfirmBooking",
                new
                {
                    BookingId = confirm.BookingId,
                    RazorpayOrderId = confirm.RazorpayOrderId,
                    RazorpayPaymentId = confirm.RazorpayPaymentId,
                    RazorpaySignature = confirm.RazorpaySignature,
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
                    UserId = cancel.UserId,
                    Reason = cancel.Reason
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<BookingReceiptDto>GetBookingReceiptDetailsAsync(int bookingId)
        {
            using var con = _dbFactory.CreateConnection();

            var result =
                await con.QueryFirstOrDefaultAsync<BookingReceiptDto>(
                    "sp_GetBookingReceiptDetails",
                    new
                    {
                        BookingId = bookingId
                    },
                    commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task SaveReceiptAsync(int bookingId,string receiptUrl)
        {
            using var con = _dbFactory.CreateConnection();

            await con.ExecuteAsync(
                "sp_SaveReceipt",
                new
                {
                    BookingId = bookingId,
                    ReceiptUrl = receiptUrl
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> VacateAsync(VacateRequestDTO request)
        {
            using var con = _dbFactory.CreateConnection();

            var result =
                await con.QueryFirstAsync<int>(
                    "sp_Pg_Vacate",
                    new
                    {
                        BookingId = request.BookingId
                    },
                    commandType: CommandType.StoredProcedure);

            return result;
        }
    }
}
