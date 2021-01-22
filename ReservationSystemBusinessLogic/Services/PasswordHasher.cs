using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ReservationSystemBusinessLogic.Services
{
    public static class PasswordHasher
    {
        public static string Create(string value)
        {
            string salt = "globglogabgalab";
            byte[] valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool Validate(string value, string hash)
            => Create(value) == hash;
    }
}
