using System;

namespace msn2.net.Configuration
{
	/// <summary>
	/// Summary description for IServerConfig.
	/// </summary>
	public interface IServerConfig
	{
		event UpdateEventDelegate UpgradeEvent1;
	}

	public delegate void UpdateEventDelegate(object sender, EventArgs e);
}
