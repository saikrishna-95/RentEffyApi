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

        public RazorpayService(IConfiguration config)
        {
            _key = config["Razorpay:Key"];
            _secret = config["Razorpay:Secret"];
        }

        public Order CreateOrder(decimal amount, string receipt)
        {
            var client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 },
                { "currency", "INR" },
                { "receipt", receipt }
            };
            return client.Order.Create(options);
        }
        public bool VerifyPayment(string orderId, string paymentId, string signature)
        {
            var payload = $"{orderId}|{paymentId}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var generated = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return generated == signature;
        }
    }
}
