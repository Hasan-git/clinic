using System.IO;

namespace Clinic.Common.Core.Extensions
{
    public static class DirectoryExtensions
    {
        public static string Up(this string directory)
        {
            if (string.IsNullOrEmpty(directory))
                return "";

            return Path.GetDirectoryName(directory);
        }
    }
}
