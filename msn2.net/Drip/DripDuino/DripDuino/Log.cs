using System;
using Microsoft.SPOT;
using System.IO;
using System.Text;

namespace DripDuino
{
    class Log
    {
        readonly static string BasePath = @"\SD\DripDuinoLog";
        static Log()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }

        public static void AddEntry(string message)
        {
            string logPath = Path.Combine(BasePath, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            using (FileStream stream = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                string text = DateTime.Now.ToString("M/d/yy h:mm tt") + ": " + message + "\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
        }

        public static string GetLog(DateTime date)
        {
            string log = string.Empty;
            string logPath = Path.Combine(BasePath, date.ToString("yyyy-MM-dd") + ".txt");
            if (File.Exists(logPath))
            {
                using (StreamReader reader = new StreamReader(logPath))
                {
                    log = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return log;
        }

    }
}
