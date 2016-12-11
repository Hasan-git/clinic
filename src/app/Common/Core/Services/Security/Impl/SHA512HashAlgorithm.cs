using System;
using System.Security.Cryptography;
using System.Text;

namespace Clinic.Common.Core.Services.Security.Impl
{
    public class SHA512HashAlgorithm : IHashAlgorithm
    {
        public string ComputeHash(string input)
        {
            var sha = new SHA512Managed();

            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            var base64Hash = Convert.ToBase64String(hash);

            return base64Hash;
        }
    }

}
