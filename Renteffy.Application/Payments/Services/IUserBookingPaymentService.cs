using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Payments.Services
{
    public interface IUserBookingPaymentService
    {
        Task<object> CreateBookingOrderAsync(CreateBookingRequestDTO request);
        Task<bool> VerifyPaymentAsync(VerifyPaymentRequestDto request);
    }
}
