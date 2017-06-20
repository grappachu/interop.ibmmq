using System;

namespace Grappachu.Interop.IbmMQ
{
    /// <summary>
    /// 
    /// </summary>
    public static class IbmMqConnectionTypeParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringvalue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IbmMqConnectionType Parse(string stringvalue)
        {
            IbmMqConnectionType val;
            int res;
            if (int.TryParse(stringvalue, out res))
            {
                return (IbmMqConnectionType)res;
            }
            if (Enum.TryParse(stringvalue, true, out val))
            {
                return val;
            }

            var acceptableValues = Enum.GetNames(typeof(IbmMqConnectionType));
            throw new ArgumentException(string.Format("Invalid Connection Type {0}. Must be one of ({1})", stringvalue, string.Join(", ", acceptableValues)));
        }
    }
}