using System;
using System.Collections.Generic;
using System.Text;
using Razorpay.Api;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Payments
{
    public interface IRazorpayService
    {
        Order CreateOrder(decimal amount, string receipt);
        bool VerifyPayment(string orderId, string paymentId, string signature);
    }
}
