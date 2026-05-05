using Renteffy.Domain.DTOs.UserTrans.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.User
{
    public interface IUserBookingsAndPaymentsApplication
    {
        Task<int> CreateBookingAsync(CreateBookingRequestDTO request);
        Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm);
        Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel);
    }
}
