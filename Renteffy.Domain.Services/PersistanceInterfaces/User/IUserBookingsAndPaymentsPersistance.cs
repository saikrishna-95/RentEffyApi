using Renteffy.Domain.DTOs.UserTrans.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.User
{
    public interface IUserBookingsAndPaymentsPersistance
    {
        Task<int> CreateBookingAsync(CreateBookingRequestDTO request);
        Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm);
        Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel);
    }
}
