using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using Renteffy.Domain.Services.PersistanceInterfaces.Payments;
using System.Security.Cryptography;
using System.Text;

namespace Renteffy.Integration.Payments
{
    public class RazorpayService : IRazorpayService
    {
        private readonly string _key;
        private readonly string _secret;
        private readonly RazorpayClient _client;
        private readonly IConfiguration _configuration;

        public RazorpayService(IConfiguration config)
        {
            _configuration = config;

            _key = _configuration["Razorpay:Key"];
            _secret = _configuration["Razorpay:Secret"];

            _client = new RazorpayClient(_key, _secret);
        }

        public Order CreateOrder(decimal amount, string receipt)
        {
            //var client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 },
                { "currency", "INR" },
                { "receipt", receipt },
                { "payment_capture", 1 }
            };
            return _client.Order.Create(options);
        }
        public bool VerifyPayment(string orderId, string paymentId, string signature)
        {
            var payload = $"{orderId}|{paymentId}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var generated = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return generated == signature;
        }

        public string CreateRefund(string paymentId,decimal amount,string? reason = null)
        {
            var options = new Dictionary<string, object>{
                            //{ "payment_id", paymentId },
                            { "amount", (int)(amount * 100) },
                            { "speed", "normal" }
                        };

            if (!string.IsNullOrWhiteSpace(reason))
            {
                options.Add("notes",new Dictionary<string, string>{{ "reason", reason }});
            }

            var refund = _client.Payment.Fetch(paymentId).Refund(options);

            return refund["id"].ToString() ?? throw new Exception("Razorpay refund ID not returned.");
        }

    }
}
