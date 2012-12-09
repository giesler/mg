namespace XMedia
{
    using System;
    using System.Collections;
	using System.ComponentModel;
    using System.Core;
    using System.Configuration.Install;

    [RunInstaller(true)]
    public class XMServerInstaller : System.Configuration.Install.Installer
    {
		private System.ServiceProcess.ServiceInstaller mInstaller;
		private System.ServiceProcess.ServiceProcessInstaller mProcessInstaller;

        public XMServerInstaller()
        {
			//create installers
			mInstaller = new System.ServiceProcess.ServiceInstaller();
			mProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();

			//setup installer properties
			mInstaller.ServiceName = "XMedia Server";
			mProcessInstaller.Username = null;
			mProcessInstaller.Password = null;

			//register installers
			Installers.Add(mProcessInstaller);
			Installers.Add(mInstaller);
		}
    }
}
