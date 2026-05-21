using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Services
{
    public interface IReceiptService
    {
        Task<string> GenerateReceiptAsync(BookingReceiptDto booking);
    }
}
