using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace msn2.net.ShoppingList
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new MainForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled exception - tell Mike!: " + e.ExceptionObject.ToString());
        }
    }
}