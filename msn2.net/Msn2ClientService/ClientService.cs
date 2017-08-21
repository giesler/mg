using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Msn2ClientService
{
    public class ClientService : IClientService
    {
        [DllImport("PowrProf.dll")]
        public static extern bool SetSuspendState(bool Hibernate, bool ForceCritical, bool DisableWakeEvent);

        public void Suspend()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.BackgroundSuspend), new object());
        }

        void BackgroundSuspend(object sender)
        {
            Thread.Sleep(1000);
            SetSuspendState(false, true, false);
        }

        public string GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }
    }
}
