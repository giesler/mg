using System;
using System.Windows.Forms;
using System.Globalization;
using msn2.net.Pictures;
using msn2.net.Pictures.Controls;

namespace msn2.net.Pictures
{
    static class PictureScreenSaverProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            PictureConfig config = PictureConfig.Load();
            if (PicContext.LoginWindowsUser(config) == true)
            {
                if (args.Length > 0)
                {
                    // Get the 2 character command line argument
                    string arg = args[0].ToLower(CultureInfo.InvariantCulture).Trim().Substring(0, 2);
                    switch (arg)
                    {
                        case "/c":
                            // Show the options dialog
                            ShowOptions();
                            break;
                        case "/p":
                            // Don't do anything for preview
                            break;
                        case "/s":
                            // Show screensaver form
                            ShowScreenSaver();
                            break;
                        default:
                            MessageBox.Show("Invalid command line argument :" + arg, "Invalid Command Line Argument", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    // If no arguments were passed in, show the screensaver
                    ShowScreenSaver();
                }
            }
        }


        static void ShowOptions()
        {
            Options options = new Options();
            Application.Run(options);
        }

        static void ShowScreenSaver()
        {
            PictureScreenSaver ss = new PictureScreenSaver();
            ss.Interval = Properties.Settings.Default.SlideshowInterval;
            ss.Path = Properties.Settings.Default.Path;
            ss.GroupId = Properties.Settings.Default.Group;
            Application.Run(ss);
        }
    }
}