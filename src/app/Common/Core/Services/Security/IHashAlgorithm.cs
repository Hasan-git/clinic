namespace Clinic.Common.Core.Services.Security
{
    public interface IHashAlgorithm
    {
        string ComputeHash(string input);
    }
}
