using System;
using System.Collections.Generic;
using System.Text;
using msn2.net.Common;

namespace msn2.net.Pictures
{
    public class PictureStorageManager: ServerMarshalByRefObject
    {
        #region Static remoting defaults

        public static int GetDefaultPort()
        {
            return 8111;
        }

        public static string GetDefaultApplicationName()
        {
            return "PictureStorageManager";
        }

        public static string GetDefaultObjectUri()
        {
            return "Manager0";
        }

        public static string GetUri(string machineName)
        {
            string storageManagerUri = string.Format(
                @"tcp://{0}:{1}/{2}/{3}",
                machineName,
                GetDefaultPort(),
                GetDefaultApplicationName(),
                GetDefaultObjectUri());

            return storageManagerUri;
        }

        #endregion
    }
}
