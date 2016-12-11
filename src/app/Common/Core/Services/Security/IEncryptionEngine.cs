namespace Clinic.Common.Core.Services.Security
{
    public interface IEncryptionEngine
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}
