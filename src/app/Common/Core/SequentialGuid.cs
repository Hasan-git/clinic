using System;
using System.Runtime.InteropServices;

namespace Clinic.Common.Core
{
    public static class SequentialGuid
    {
        const int RPC_S_OK = 0;

        [DllImport("rpcrt4.dll", SetLastError = true)]
        private static extern int UuidCreateSequential(out Guid guid);


        public static Guid Generate()
        {
            Guid guid;
            int result = UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
                return guid;
            else
                return GenerateFallBack();
        }

        private static Guid GenerateFallBack()
        {
            var tempGuid = Guid.NewGuid();
            var bytes = tempGuid.ToByteArray();
            var time = DateTime.Now;
            bytes[3] = (byte)time.Year;
            bytes[2] = (byte)time.Month;
            bytes[1] = (byte)time.Day;
            bytes[0] = (byte)time.Hour;
            bytes[5] = (byte)time.Minute;
            bytes[4] = (byte)time.Second;
            return new Guid(bytes);
        }
    }
}