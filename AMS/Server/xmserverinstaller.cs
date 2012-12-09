namespace XMedia
{
    using System;
    using System.Collections;
	using System.ComponentModel;
    using System.Configuration.Install;
	using System.ServiceProcess;

    [RunInstaller(true)]
    public class XMServerInstaller : Installer
    {
		private ServiceInstaller mInstaller;
		private ServiceProcessInstaller mProcessInstaller;

        public XMServerInstaller()
        {
			//create installers
			mInstaller = new ServiceInstaller();
			mProcessInstaller = new ServiceProcessInstaller();

			//setup installer properties
			mProcessInstaller.Account = ServiceAccount.LocalSystem;
			mInstaller.ServiceName = "XMedia Server";
			mInstaller.StartType = ServiceStartMode.Automatic;

			//register installers
			Installers.Add(mProcessInstaller);
			Installers.Add(mInstaller);
		}
    }
}
