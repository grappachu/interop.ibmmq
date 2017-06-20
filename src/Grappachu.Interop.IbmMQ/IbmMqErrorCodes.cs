using System.Collections.Generic;

namespace Grappachu.Interop.IbmMQ
{
    internal static class IbmMqErrorCodes
    {
        private static readonly Dictionary<int, string> Codes = new Dictionary<int, string>();

        static IbmMqErrorCodes()
        {
            Codes.Add(2053, "Queue limit exceeded");
            Codes.Add(2058, "Invalid Queue Manager name");
            Codes.Add(2059, "Queue Manager is not available");
        }

        public static string GetMessage(int errorCode)
        {
            return Codes[errorCode];
        }
    }
}