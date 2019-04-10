using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace memoseeds.Models
{
    public class HashPassword
    {
        private static byte[] Salt { set; get; }
        public IConfiguration Configuration { set; get; }

        public HashPassword(IConfiguration configuration)
        {
            Configuration = configuration;
            Salt = Encoding.ASCII.GetBytes(Configuration["Jwt:Issuer"]);
        }

        public static string Encrypt(string password, string salt)
        {
            return prepareEncrypted(
                password,
                Encoding.ASCII.GetBytes(
                    salt + Encoding.ASCII.GetString(Salt)
                    )
                );
        }

        public static string Encrypt(string password)
        {
            return prepareEncrypted(password, Salt);
        }

        private static string prepareEncrypted(string password, byte[] salt)
        {
            string
            hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
