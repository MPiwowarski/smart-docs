using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace SmartDocs.Services
{
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string message)
        {
            byte[] input;
            using (HashAlgorithm alghorithm = SHA256.Create())
            {
                input = alghorithm.ComputeHash(Encoding.UTF8.GetBytes(message));
            }

            var sb = new StringBuilder();
            foreach (byte b in input)
            {
                // "X2" indicates the string should be formatted in Hexadecimal
                // eg. No format string: 13, 'X2' format string: 0D
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
