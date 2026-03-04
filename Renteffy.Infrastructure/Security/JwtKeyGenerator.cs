using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Renteffy.Infrastructure.Security
{
    public interface IJwtKeyGenerator
    {
        string GenerateKey(string personalData, int bytesLength = 64);
    }
    public class JwtKeyGenerator : IJwtKeyGenerator
    {
        public string GenerateKey(string personalData, int bytesLength = 64)
        {
            if (bytesLength < 32)
                throw new ArgumentException("Key length should be at least 32 bytes for security.");

            using var sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(personalData));
            string securityKey = Convert.ToBase64String(hash);
            return securityKey;
        }
    }
}
