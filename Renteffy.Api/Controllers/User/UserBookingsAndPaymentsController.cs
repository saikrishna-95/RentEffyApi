using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;
using Renteffy.Domain.Services.PersistanceInterfaces.Services;
using Renteffy.Persistence.RegistrationDbContext;
using QuestPDF.Infrastructure;

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
        private readonly IEmailService _emailService;
        private readonly IReceiptService _receiptService;

        public UserBookingsAndPaymentsController(IUserBookingsAndPaymentsApplication readApp, AppDbContext context, IMemoryCache cache,IRazorpayService razorpay, IConfiguration config,
            IEmailService emailService, IReceiptService receiptService)
        {
            _readApp = readApp;
            _context = context;
            _cache = cache;
            _razorpay = razorpay;
            _config = config;
            _emailService = emailService;
            _receiptService = receiptService;
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("BookingPg")]
        public async Task<IActionResult> BookingPg(CreateBookingRequestDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "User not found."
                    });
                }
                var userId = int.Parse(userIdClaim);

                var bookingResult = await _readApp.CreateBookingAsync(request, userId);
                var order = _razorpay.CreateOrder(bookingResult.Price, bookingResult.BookingId.ToString());
                var razorpayOrderId = order["id"]?.ToString();
                if (string.IsNullOrWhiteSpace(razorpayOrderId))
                {
                    return BadRequest(new{success = false,message = "Razorpay order was not created."});
                }

                var saved = await _readApp.SaveRazorpayOrderAsync(bookingResult.BookingId,razorpayOrderId);

                if (!saved)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to save Razorpay order."
                    });
                }
                return Ok(new
                {
                    success = true,
                    BookingId=bookingResult.BookingId,
                    orderId = order["id"].ToString(),
                    amount = Convert.ToDecimal(order["amount"]) / 100,
                    key = _config["Razorpay:Key"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("ConfirmPgBooking")]
        public async Task<IActionResult> ConfirmPgBooking(ConfirmBookingRequestDTO request)
        {
            try
            {
                var userIdValue = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrWhiteSpace(userIdValue))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "User ID not found in token."
                    });
                }
                var userId = int.Parse(userIdValue);
                var booking = await _readApp.GetBookingForPaymentVerificationAsync(request.BookingId,userId);
                if (booking == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Booking not found."
                    });
                }
                // 3. Make sure Razorpay order exists
                if (string.IsNullOrWhiteSpace(booking.RazorpayOrderId))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Razorpay order ID not found for this booking."
                    });
                }
                var isValid = _razorpay.VerifyPayment(booking.RazorpayOrderId,request.RazorpayPaymentId,request.RazorpaySignature);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid payment signature"
                    });
                }
                request.RazorpayOrderId = booking.RazorpayOrderId;
                var result = await _readApp.ConfirmBookingAsync(request);
                if (result != 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to confirm booking"
                    });
                }
                var bookingRes = await _readApp.GetBookingReceiptDetailsAsync(request.BookingId);
                try
                {
                    QuestPDF.Settings.License = LicenseType.Community;
                    var receiptPath = await _receiptService.GenerateReceiptAsync(bookingRes);

                    await _readApp.SaveReceiptAsync(request.BookingId, receiptPath.CloudUrl);

                    await _emailService.SendEmailAsync(bookingRes.Email, "Booking Confirmed", "<h2>Your booking has been confirmed.</h2>", receiptPath.LocalPath);
                }
                catch (Exception ex)
                {
                    // Optional logging
                    Console.WriteLine(ex.Message);
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

        //[Authorize]
        //[HttpPost("CancelPgBooking")]
        //public async Task<IActionResult> Cancel(CancelBookingRequestDTO request)
        //{
        //    try
        //    {
        //       var result = await _readApp.CancelBookingAsync(request);

        //        return Ok(new
        //        {
        //            success = result == 1,
        //            message = result == 1 ? "Cancelled" : "Failed"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("CancelPgBooking")]
        public async Task<IActionResult> Cancel(CancelBookingRequestDTO request)
        {
            try
            {
                var result =
                    await _readApp.CancelBookingAsync(request);

                return Ok(new
                {
                    success = result == 1,
                    message = result == 1
                        ? "Booking cancelled and refund initiated successfully"
                        : "Failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("TestEmail")]
        public async Task<IActionResult> TestEmail()
        {
            await _emailService.SendEmailAsync(
                "test@gmail.com",
                "Test Mail",
                "<h1>Email Working</h1>");

            return Ok("Mail Sent");
        }

        [HttpPost("Vacate")]
        public async Task<IActionResult>Vacate(VacateRequestDTO request)
        {
            try
            {
                var result = await _readApp.VacateAsync(request);
                return Ok(new
                {
                    success = result == 1,
                    message = result == 1 ? "Vacated Successfully" : "Failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserBookings/{userId}")]
        public async Task<IActionResult> GetMyBookings(int userId)
        {
            var result = await _readApp.GetMyBookingsAsync(userId);
            return Ok(new
            {
                success = true,
                message = "Bookings fetched successfully",
                data = result
            });
        }
    }
}
