using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NLog;
using NLog.Fluent;

namespace Gip.Utils
{
    public static class utils
    {
        //public string hashPassword(String plain_text_password)
        //{
        //    string password = plain_text_password;
 
        //    // generate a 128-bit salt using a secure PRNG
        //    byte[] salt = new byte[128 / 8];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }

        //    // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA1,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8));
        //    return hashed;
        //}

        public static void log(string message, string[] properties) 
        {
            var logger = LogManager.GetCurrentClassLogger();

            logger.Info().Message(message).Property(properties[0], properties[1]).Write();
        }
    }
    
}