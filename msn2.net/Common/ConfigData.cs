using System;

namespace msn2.net.Common
{

	/// <summary>
	/// Base class for data stored in config system
	/// </summary>
	public class ConfigData
	{
		private Guid itemKey = Guid.Empty;

		public virtual int IconIndex
		{
			get { return 0; }
		}

		public Guid ItemKey
		{
			get { return itemKey; }
			set { itemKey = value; }
		}
	}



}
