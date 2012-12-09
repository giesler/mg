using System;
using System.Diagnostics;

namespace XMedia
{
	/// <summary>
	/// Helper functions for logging data
	/// </summary>
	public class XMLog
	{
		public const string SourceName = "XMServer";
		public const string LogName = "XMServerLog";

		/// <summary>
		/// Perform one-time startup routines for custom logging.
		/// </summary>
		public static void StaticInit()
		{
			//register ourselves as an event source
			//EventLog.CreateEventSource(SourceName, LogName);

			//create the event log
			mLog = new EventLog(LogName, ".", SourceName);
		}
		private static EventLog mLog;

		/// <summary>
		/// Write a text string to the debug console and event log.
		/// </summary>
		/// <param name="Message">String to output.</param>
		public static void WriteLine(string Message)
		{
			//Add default module and entry type
			WriteLine(Message, "XMServer", EventLogEntryType.Information);
		}
		/// <summary>
		/// Write a text string to the debug console and event log.
		/// </summary>
		/// <param name="Message">String to output.</param>
		/// <param name="Module">Module name prepended to the output.</param>
		public static void WriteLine(string Message, string Module)
		{
			//Add default entry type
			WriteLine(Message, Module, EventLogEntryType.Information);
		}
		/// <summary>
		/// Write a text string to the debug console and event log.
		/// </summary>
		/// <param name="Message">String to output.</param>
		/// <param name="Module">Module name prepended to the output.</param>
		/// <param name="Type">Type and severity of message.</param>
		public static void WriteLine(string Message, string Module, EventLogEntryType Type)
		{
			//First, write to the console
			Trace.WriteLine(Message, Module);

			//Now add an event log entry
			mLog.WriteEntry(Module + ": " + Message, Type);
		}
	}
}
