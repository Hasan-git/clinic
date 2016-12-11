using System;
using System.Text;

namespace Clinic.Common.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string FullMessage(this Exception exception)
        {
            return exception.FullMessage("");
        }

        public static string FullMessage(this Exception exception, string message)
        {
            var builder = new StringBuilder(message);
            var inner = exception;
            while (inner != null)
            {
                builder.Append(inner.Message).Append(", ");
                inner = inner.InnerException;
            }
            if (builder.Length > message.Length) builder.Length = builder.Length - 2;
            return builder.ToString();

        }
    }
}
