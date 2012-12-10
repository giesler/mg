using System;
using System.Collections.Generic;
using System.Text;

namespace msn2.net.Common
{
    public class ServerMarshalByRefObject: MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Returns current system time in UTC
        /// </summary>
        /// <returns></returns>

        public DateTime Ping()
        {
            return DateTime.Now.ToUniversalTime();
        }

    }
}
