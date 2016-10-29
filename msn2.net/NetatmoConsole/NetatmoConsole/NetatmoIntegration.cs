using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    public class NetatmoIntegration
    {
        public static DateTime GetDate(double milliseconds)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = epoch.AddSeconds(milliseconds).ToLocalTime();
            return dt;
        }

        public static float GetFahrenheit(float celsius)
        {
            return (celsius * 9 / 5) + 32;
        }
    }
}
