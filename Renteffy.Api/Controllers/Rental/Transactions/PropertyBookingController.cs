using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Application.Interfaces.Rental;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;

namespace Renteffy.Api.Controllers.Rental.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyBookingController : ControllerBase
    {
        private readonly IPropertyBookingApplication _application;
        private readonly IRazorpayService _razorpay;
        public PropertyBookingController(IPropertyBookingApplication application,IRazorpayService razorpay)
        {
            _application = application;
            _razorpay = razorpay;
        }

        [Authorize]
        [HttpPost("CreatePropertyBooking")]
        public async Task<IActionResult> CreatePropertyBooking(CreatePropertyBookingRequestDto request)
        {
            try
            {
                var userId = request.UserID;
                var result = await _application.CreateBookingAsync(userId,request);

                return Ok(new
                {
                    success = true,
                    PropertyBookingID = result.PropertyBookingID,
                    amount = result.Amount,
                    currency = result.Currency,
                    orderId = result.RazorpayOrderID,
                    key = result.RazorpayKey
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

        [Authorize]
        [HttpPost("ConfirmPropertyBooking")]
        public async Task<IActionResult> ConfirmPropertyBooking(ConfirmPropertyBookingRequestDto request)
        {
            try
            {
                var userId = request.UserID;
                var isValid =  _razorpay.VerifyPayment(request.RazorpayOrderID,request.RazorpayPaymentID,request.RazorpaySignature);

                if (!isValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid payment signature."
                    });
                }

                var result = await _application.ConfirmBookingAsync(userId,request);

                if (result != 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to confirm booking."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Property booking confirmed successfully."
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

        [Authorize]
        [HttpPost("CancelPropertyBooking")]
        public async Task<IActionResult> CancelPropertyBooking(CancelPropertyBookingRequestDto request)
        {
            try
            {
                var userId = request.UserID;
                var result = await _application.CancelBookingAsync(userId, request);

                if (result != 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to cancel booking."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Booking cancelled and refund initiated."
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

        [Authorize]
        [HttpPost("OwnerCancelPropertyBooking")]
        public async Task<IActionResult> OwnerCancelPropertyBooking(CancelPropertyBookingRequestDto request)
        {
            try
            {
                var ownerId = request.UserID;
                var result = await _application.OwnerCancelBookingAsync(ownerId,request);

                if (result != 1)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Unable to cancel booking."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Booking cancelled and refund initiated."
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

        [Authorize]
        [HttpGet("GetMyPropertyBookings")]
        public async Task<IActionResult> GetMyPropertyBookings(int userId)
        {
            try
            {
                var result = await _application.GetUserBookingsAsync(userId);
                return Ok(new
                {
                    success = true,
                    data = result
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

        [Authorize]
        [HttpGet("GetOwnerPropertyBookings")]
        public async Task<IActionResult> GetOwnerPropertyBookings(int ownerId)
        {
            try
            {
                var result = await _application.GetOwnerBookingsAsync(ownerId);

                return Ok(new
                {
                    success = true,
                    data = result
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

        [Authorize]
        [HttpGet("GetPropertyBookingDetails/{bookingId}")]
        public async Task<IActionResult> GetPropertyBookingDetails(int bookingId,int userId)
        {
            try
            {
                var result = await _application.GetBookingDetailsAsync(bookingId,userId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Booking not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = result
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
