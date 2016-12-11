using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Clinic.Common.Core.Services.Security.Impl
{
    /// <summary>
    /// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and 
    /// decrypt data. As long as encryption and decryption routines use the same
    /// parameters to generate the keys, the keys are guaranteed to be the same.
    /// The class uses static functions with duplicate code to make it easier to
    /// demonstrate encryption and decryption logic.
    /// </summary>

    public class AesEncryptionEngine : IEncryptionEngine
    {
        private const string EncryptionPassword = "password";
        private const string EncryptionSalt = "password";
        private const int EncryptionPasswordIterations = 2;
        private const string EncryptionInitVector = "passwordpassword";
        private const int EncryptionKeySize = 128;

        public string Decrypt(string input)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(EncryptionInitVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(EncryptionSalt);

            var cipherTextBytes = Convert.FromBase64String(input);

            var password = new Rfc2898DeriveBytes(EncryptionPassword, saltValueBytes, EncryptionPasswordIterations);

            var keyBytes = password.GetBytes(EncryptionKeySize / 8);

            var symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            var memoryStream = new MemoryStream(cipherTextBytes);

            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public string Encrypt(string plainText)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(EncryptionInitVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(EncryptionSalt);

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var password = new Rfc2898DeriveBytes(EncryptionPassword, saltValueBytes, EncryptionPasswordIterations);

            var keyBytes = password.GetBytes(EncryptionKeySize / 8);

            var symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            var cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            var cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }
    }

}
