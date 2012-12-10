using System;

namespace msn2.net.Configuration
{
	/// <summary>
	/// Summary description for IMachineConfig.
	/// </summary>
	public interface IMachineConfig
	{
		string ServerName
		{
			get;
			set;
		}

		string ConnectionString
		{
			get;
			set;
		}
	}
}
