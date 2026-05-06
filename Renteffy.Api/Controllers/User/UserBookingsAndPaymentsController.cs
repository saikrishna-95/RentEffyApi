using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;
using Renteffy.Persistence.RegistrationDbContext;

namespace Renteffy.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingsAndPaymentsController : ControllerBase
    {
        private readonly IUserBookingsAndPaymentsApplication _readApp;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;
        private readonly IRazorpayService _razorpay;
        private readonly IConfiguration _config;
        public UserBookingsAndPaymentsController(IUserBookingsAndPaymentsApplication readApp, AppDbContext context, IMemoryCache cache,IRazorpayService razorpay, IConfiguration config)
        {
            _readApp = readApp;
            _context = context;
            _cache = cache;
            _razorpay = razorpay;
            _config = config;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("BookingPg")]
        public async Task<IActionResult> BookingPg(CreateBookingRequestDTO request)
        {
            try
            {
                var bookingId = await _readApp.CreateBookingAsync(request);
                var order = _razorpay.CreateOrder(request.Price, bookingId.ToString());
                return Ok(new
                {
                    success = true,
                    BookingId=bookingId,
                    orderId = order["id"].ToString(),
                    amount = order["amount"],
                    key = _config["Razorpay:Key"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("ConfirmPgBooking")]
        public async Task<IActionResult> ConfirmPgBooking(ConfirmBookingRequestDTO request)
        {
            try
            {
                var isValid = _razorpay.VerifyPayment(request.OrderId,request.PaymentId,request.Signature);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid payment signature"
                    });
                }
                var result = await _readApp.ConfirmBookingAsync(request);
                if (result == 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to confirm booking"
                    });
                }
                return Ok(new
                {
                    success = true,
                    message = "Booking confirmed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("CancelPgBooking")]
        public async Task<IActionResult> Cancel(CancelBookingRequestDTO request)
        {
            try
            {
               var result = await _readApp.CancelBookingAsync(request);

                return Ok(new
                {
                    success = result == 1,
                    message = result == 1 ? "Cancelled" : "Failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
