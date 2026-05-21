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
                    amount = Convert.ToDecimal(order["amount"]) / 100,
                    key = _config["Razorpay:Key"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("ConfirmPgBooking")]
        public async Task<IActionResult> ConfirmPgBooking(ConfirmBookingRequestDTO request)
        {
            try
            {
                var isValid = _razorpay.VerifyPayment(request.RazorpayOrderId,request.RazorpayPaymentId,request.RazorpaySignature);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid payment signature"
                    });
                }
                var result = await _readApp.ConfirmBookingAsync(request);
                if (result != 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to confirm booking"
                    });
                }
                var booking = await _readApp.GetBookingReceiptDetailsAsync(request.BookingId);

                var receiptPath = await _receiptService.GenerateReceiptAsync(booking);

                await _readApp.SaveReceiptAsync(request.BookingId,receiptPath);

                await _emailService.SendEmailAsync(booking.Email,"Booking Confirmed","<h2>Your booking has been confirmed.</h2>",receiptPath);
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
                    message = result == 1
                        ? "Vacated Successfully"
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
    }
}
