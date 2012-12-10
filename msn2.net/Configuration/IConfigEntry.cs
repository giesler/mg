using System;

namespace msn2.net.Configuration
{
	/// <summary>
	/// Summary description for IConfigEntry
	/// </summary>
	public interface IConfigEntry
	{
		string ConfigName
		{
			get;
			set;
		}

		Guid ConfigId
		{
			get;
			set;
		}
	}
}
