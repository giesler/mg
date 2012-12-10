using System;

namespace msn2.net.Configuration
{
	/// <summary>
	/// Summary description for IUserConfig.
	/// </summary>
	public interface IUserConfig
	{
		string UserName
		{
			get;
			set;
		}

		Guid UserId
		{
			get;
			set;
		}
	}
}
