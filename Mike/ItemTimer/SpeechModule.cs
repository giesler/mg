using System;
using System.Threading;
using SpeechLib;
using System.Windows.Forms;

namespace ItemTimer
{
	/// <summary>
	/// Summary description for SpeechModule.
	/// </summary>
	public class SpeechModule
	{
		public SpeechModule()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void Speak(string text)
		{
			try
			{

				SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
				SpVoice speech = new SpVoice();
				speech.Speak(text, spFlags);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error speaking: " + ex.Message);
			}
		}
	}
}
