using System;
using System.Configuration;

namespace vbsw
{
	/// <summary>
	/// Summary description for Utilities.
	/// </summary>
	public class Config
	{
		private static string connectionString;
		private static string smtpServer;

		public static string ConnectionString 
		{
			get 
			{
				if (connectionString == null) 
				{
					connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
				}
				return connectionString;
			}
		}

		public static string SmtpServer 
		{
			get 
			{
				if (smtpServer == null) 
				{
					smtpServer = ConfigurationSettings.AppSettings["SmtpServer"];
				}
				return smtpServer;
			}
		}

	}
}
